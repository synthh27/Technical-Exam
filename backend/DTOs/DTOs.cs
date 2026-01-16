namespace TaskManager.DTOs
{
    public class DTOs
    {
        public record AuthRequest
        (
            string Enail,
            string Password
        );

        public record AuthResponse
        (
            int userId,
            string Email,       
            string Token
        );
    }
}
