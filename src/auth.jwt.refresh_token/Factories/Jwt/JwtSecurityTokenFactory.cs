using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace auth.jwt.refresh_token.Factories.Jwt
{
    public static class JwtSecurityTokenFactory
    {
        public static JwtSecurityToken Create(string issuer, string audience, IEnumerable<Claim> claims, DateTime notBefore, DateTime expires, SigningCredentials signingCredentials)
            => new(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: notBefore,
                expires: expires,
                signingCredentials: signingCredentials
                );
    }
}
