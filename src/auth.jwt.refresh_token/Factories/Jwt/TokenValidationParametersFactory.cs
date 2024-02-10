using Microsoft.IdentityModel.Tokens;

namespace auth.jwt.refresh_token.Factories.Jwt
{
    public static class TokenValidationParametersFactory
    {
        public static TokenValidationParameters Create(string validIssuer, string validAudience, SecurityKey issuerSigningKey, TimeSpan clockSkew, bool validateIssuer = true, bool validateAudience = true, bool validateLifetime = true, bool validateIssuerSigningKey = true)
            => new()
            {
                ValidIssuer = validIssuer,
                ValidAudience = validAudience,
                IssuerSigningKey = issuerSigningKey,
                ClockSkew = clockSkew,
                ValidateIssuer = validateIssuer,
                ValidateAudience = validateAudience,
                ValidateLifetime = validateLifetime,
                ValidateIssuerSigningKey = validateIssuerSigningKey,
            };
    }
}
