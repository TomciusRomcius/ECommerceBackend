namespace ECommerce.Common.Utils
{
    public record class JwtOptions(
        string Issuer,
        string SigningKey,
        int ExpirationHours
    );
}
