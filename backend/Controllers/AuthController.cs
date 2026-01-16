using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using TaskManager.Data;
using TaskManager.DTOs;
using TaskManager.Models;
using TaskManager.Services;
namespace TaskManager.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtService _jwtService;

        public AuthController(ApplicationDbContext context, JwtService jwtService)
        {   
            _context = context;
            _jwtService = jwtService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            // CHECKS IF USER EXISTS
            var user = await _context.Users.FindAsync(id);

            // RETURNS 404 IF USER NOT FOUND
            if (user == null) return NotFound();

            // RETURNS USER DATA
            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthRequest request)
        {
            // CHECKS IF USER EXISTS
            var isExistingUser = await _context.Users.AnyAsync(u => u.Email == request.Email);

            // RETURNS 404 CONFLIC IF EMAIL IS ALREADY REGISTERED
            if (isExistingUser) return Conflict("Email is already registered.");

            // CREATE NEW USER
            var newUser = new User
            {
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Email = request.Email
            };

            // ADDS NEW USER TO DATABASE
            try
            {
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // RETURNS 400 ERROR IF SOMETHING WENT WRONG
                return BadRequest("An error occurred while registering the user.");
            }

            var token = _jwtService.GenerateToken(newUser);
            // RETURNS SUCCESS RESPONSE
            return CreatedAtAction(
                nameof(GetUserById),
                new { Id = newUser.Id },
                new AuthResponse(
                    "User Regustered Successfully.",
                    newUser.Id, newUser.Email, token)
                );
        }
    }
}