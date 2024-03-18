using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace auth.jwt.refresh_token.Configs
{
    public class CustomJwtSecurityTokenHandler : JsonWebTokenHandler
    {
        public override ClaimsPrincipal ValidateToken(string token, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            Debug.WriteLine("-------------------------");
            return base.ValidateToken(token, validationParameters, out validatedToken);
        }
    }
}
