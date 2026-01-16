using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using TaskManager.Controllers.Base;
using TaskManager.Data;
using TaskManager.DTOs;
using TaskManager.Models;
namespace TaskManager.API
{
    [Authorize]
    [Route("api/tasks")]
    [ApiController]
    public class TasksController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public TasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // FETCHES ALL USER'S TASKS
            var tasks = await _context.Tasks
                .Where(t => t.UserId == UserId)
                .Select(t => new TaskDTO(
                    t.Id,
                    t.Title,
                    t.IsDone
                ))
                .ToListAsync();

            // RETURNS 404 IF NO TASKS FOUND
            if (tasks.Count == 0) return NotFound("No tasks found");

            // RETURNS 200 WITH TASKS LIST
            return Ok( new GetTasksResponse(
                "Tasks fetched successfully",
                tasks));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByTaskId(int id)
        {
            // FETCHES TASK BY ID
            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == UserId);

            // RETURNS 404 IF TASKS NOT FOUND
            if (task == null) return NotFound("Task not found");
            
            // RETURNS 200 WITH TASK
            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskRequest request)
        {
            // GETS USER FROM DB
            var currUser = await _context.Users.FirstAsync(u => u.Id == UserId);

            // RETURNS 404 IF USER IS NULL
            if (currUser is null) return NotFound("User not found");

            Debug.WriteLine($"this is the emaillll: {currUser}");

            //CREATE NEW TASK            
            var newTask = new TaskItem
            {
                Title = request.Title,
                UserId = UserId
            };

            try {
                // ADDS NEW TASK TO DATABASE
                _context.Tasks.Add(newTask);
                await _context.SaveChangesAsync();

                // RETURN 201 WITH CREATED TASK
                return CreatedAtAction(
                    nameof(GetByTaskId),
                    new { id = newTask.Id },
                    new CreateTaskResponse(
                        newTask.Id,
                        newTask.Title,
                        newTask.IsDone
                    )
                );
            } catch (Exception ex) {
                // RETURN 400 IF FAILED TO SAVE TO DB
                return BadRequest("Failed to save to db.");
            }
        }

        [HttpPatch("{id}")] 
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskRequest request)
        {
            // GET THE TASK
            var task = await _context.Tasks.FindAsync(id);

            // RETURNS 404 IF TASK NOT FOUND
            if (task == null) return NotFound();

            // UPDATES THE TASK
            if (request.Title != null) task.Title = request.Title;
            if (request.IsDone.HasValue) task.IsDone = request.IsDone.Value;

            // SAVES CHANGES TO DATABASE
            await _context.SaveChangesAsync();

            return Ok("Tasks updated successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return NotFound();

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
