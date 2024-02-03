using Microsoft.IdentityModel.Tokens;

namespace auth.jwt.refresh_token.Factories.Jwt
{
    public class TokenValidationParametersFactory : IFactory<TokenValidationParameters>
    {
        public static TokenValidationParameters Create(string validIssuer,string validAudience, SecurityKey issuerSigningKey, TimeSpan clockSkew, bool ValidateIssuer = true, bool ValidateAudience = true, bool ValidateLifetime = true, bool ValidateIssuerSigningKey = true)
        {
            return  new TokenValidationParameters
            {    
                
                ValidIssuer =validIssuer,
                ValidAudience = validAudience,
                IssuerSigningKey = issuerSigningKey,
                ClockSkew = clockSkew,
                ValidateIssuer = ValidateIssuer,
                ValidateAudience = ValidateAudience,
                ValidateLifetime = ValidateLifetime,
                ValidateIssuerSigningKey = ValidateIssuerSigningKey,                
            };
        }

        public static TokenValidationParameters Create(params object[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}
