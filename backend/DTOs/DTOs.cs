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
}
