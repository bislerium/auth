using auth.jwt.refresh_token.Abstractions.Services.Jwt;
using auth.jwt.refresh_token.Dtos.Auth;
using auth.jwt.refresh_token.Options.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace auth.jwt.refresh_token.Implementations.Services.Jwt
{
    public class JwtTokenGeneratorService (IOptions<JwtOption> jwtOption): IJwtTokenGeneratorService
    {
        private readonly JwtOption _jwtOption = jwtOption.Value;

        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new();

        public async Task<JwtTokenDto> GenerateJwtToken(IEnumerable<Claim> claims)
        {
            Claim[] pre_claims = [
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64)
                ];

            var aggregated_claims = pre_claims.UnionBy(claims, x => x.Type).ToArray();

            var accessToken = Task.Run(() => GenerateAccessToken(aggregated_claims));
            var refreshToken = Task.Run(() => GenerateRefreshToken(aggregated_claims));
            await Task.WhenAll(accessToken, refreshToken);

            return new JwtTokenDto()
            {
                AccessToken = accessToken.Result,
                RefreshToken = refreshToken.Result
            };
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var token = new JwtSecurityToken(
                issuer: _jwtOption.Issuer,
                audience: _jwtOption.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow.AddSeconds(_jwtOption.NotBeforeInSeconds),
                expires: DateTime.UtcNow.AddMinutes(_jwtOption.AccessToken.ExpirationTimeInMinutes),
                signingCredentials: _jwtOption.AccessToken.SigningCredentials
            );

            return _jwtSecurityTokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken(IEnumerable<Claim> claims)
        {
            var token = new JwtSecurityToken(
                issuer: _jwtOption.Issuer,
                audience: _jwtOption.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow.AddSeconds(_jwtOption.NotBeforeInSeconds),
                expires: DateTime.UtcNow.AddDays(_jwtOption.RefreshToken.ExpirationTimeInDays),
                signingCredentials: _jwtOption.RefreshToken.SigningCredentials
            );

            return _jwtSecurityTokenHandler.WriteToken(token);
        }
    }
}
