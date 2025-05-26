namespace ECommerce.Presentation.Common.Utils;

public static class PasswordHasher
{
    public static string Hash(string text)
    {
        return BCrypt.Net.BCrypt.HashPassword(text);
    }

    public static bool Verify(string text, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(text, passwordHash);
    }
}