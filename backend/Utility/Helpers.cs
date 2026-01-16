namespace TaskManager.Utility
{
    public static class Helpers
    {
        public static string HashPassword(string password)
        {
           return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPassowrd(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
