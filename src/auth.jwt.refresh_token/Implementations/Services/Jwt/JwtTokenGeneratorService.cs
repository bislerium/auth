using auth.jwt.refresh_token.Abstractions.Services.Jwt;
using auth.jwt.refresh_token.Dtos.Auth;
using auth.jwt.refresh_token.Factories.Jwt;
using auth.jwt.refresh_token.Options.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace auth.jwt.refresh_token.Implementations.Services.Jwt
{
    public class JwtTokenGeneratorService(IOptions<JwtOption> jwtOption) : IJwtTokenGeneratorService
    {
        private readonly JwtOption _jwtOption = jwtOption.Value;

        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new();

        public JwtTokenDto GenerateJwtToken(IEnumerable<Claim> claims)
        {
            Claim[] pre_claims = [
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64)
                ];

            var aggregated_claims = pre_claims.UnionBy(claims, x => x.Type).ToArray();

            var accessToken = GenerateAccessToken(aggregated_claims);
            var refreshToken = GenerateRefreshToken(pre_claims);

            return new JwtTokenDto()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            return WriteToken(
                issuer: _jwtOption.Issuer,
                audience: _jwtOption.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow.AddSeconds(_jwtOption.NotBeforeInSeconds),
                expires: DateTime.UtcNow.AddMinutes(_jwtOption.AccessToken.ExpirationTimeInMinutes),
                signingCredentials: _jwtOption.AccessToken.SigningCredentials
            );
        }

        public string GenerateRefreshToken(IEnumerable<Claim> claims)
        {
            return WriteToken(
                issuer: _jwtOption.Issuer,
                audience: _jwtOption.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow.AddMinutes(_jwtOption.AccessToken.ExpirationTimeInMinutes),
                expires: DateTime.UtcNow.AddDays(_jwtOption.RefreshToken.ExpirationTimeInDays),
                signingCredentials: _jwtOption.RefreshToken.SigningCredentials
                );
        }

        private string WriteToken(string issuer, string audience, IEnumerable<Claim> claims, DateTime notBefore, DateTime expires, SigningCredentials signingCredentials)
        {
            var token = JwtSecurityTokenFactory.Create(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: notBefore,
                expires: expires,
                signingCredentials: signingCredentials
                );

            return _jwtSecurityTokenHandler.WriteToken(token);
        }
    }
}
