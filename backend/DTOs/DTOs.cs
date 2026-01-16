using System.Reflection;
using TaskManager.Models;

namespace TaskManager.DTOs
{
    public record AuthRequest
    (
        string Email,
        string Password
    );

    public record AuthResponse
    (
        string? Message,
        int userId,
        string Email,
        string Token
    );

    public record GetTasksResponse
    (
        string? Message,
        List<TaskItem> Tasks
    );

    public record CreateTaskRequest
    (
        string Title
    );

    public record UpdateTaskRequest
    (
        string? Title,
        bool? IsDone
    );
}
