using auth.jwt.non_refresh_token.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace auth.jwt.non_refresh_token.Services
{
    public class JwtService(IOptions<JwtOption> jwtOption)
    {
        private readonly JwtOption _jwtOption = jwtOption.Value;

        public string GenerateToken(List<Claim> claims)
        {

            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

            var token = new JwtSecurityToken(
                issuer: _jwtOption.Issuer,
                audience: _jwtOption.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow.AddSeconds(_jwtOption.NotBeforeInSeconds),
                expires: DateTime.UtcNow.AddMinutes(_jwtOption.ExpirationTimeInMinutes),
                signingCredentials: _jwtOption.SigningCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
