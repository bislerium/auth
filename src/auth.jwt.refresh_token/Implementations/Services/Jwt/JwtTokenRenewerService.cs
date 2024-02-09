using auth.jwt.refresh_token.Abstractions.Repositories;
using auth.jwt.refresh_token.Abstractions.Services.Jwt;
using auth.jwt.refresh_token.Dtos.Auth;
using auth.jwt.refresh_token.Options.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace auth.jwt.refresh_token.Implementations.Services.Jwt
{
    public class JwtTokenRenewerService (IOptions<JwtOption> jwtOption, IJwtTokenGeneratorService jwtTokenGeneratorService, ITokenRepository tokenRepository) : IJwtTokenRenewerService
    {
        private readonly JwtOption _jwtOption = jwtOption.Value;

        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new();

        private readonly string[] _generatedClaims = [
            JwtRegisteredClaimNames.Jti,
            JwtRegisteredClaimNames.Iat,
            JwtRegisteredClaimNames.Nbf,
            JwtRegisteredClaimNames.Exp,
            JwtRegisteredClaimNames.Iss,
            JwtRegisteredClaimNames.Aud
            ];

        public async Task<JwtTokenDto> Renew(JwtTokenDto jwtTokenDto)
        {
            var claims = await ValidateToken(jwtTokenDto);
            return await jwtTokenGeneratorService.GenerateJwtToken(claims);
        }

        public async Task<IEnumerable<Claim>> ValidateToken(JwtTokenDto jwtToken)
        {
            var accessTokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = _jwtOption.Issuer,
                ValidAudience = _jwtOption.Audience,
                IssuerSigningKey = _jwtOption.AccessToken.SecurityKey,
                ClockSkew = TimeSpan.FromSeconds(_jwtOption.ClockSkewInSeconds),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true
            };

            var refreshTokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = _jwtOption.Issuer,
                ValidAudience = _jwtOption.Audience,
                IssuerSigningKey = _jwtOption.RefreshToken.SecurityKey,
                ClockSkew = TimeSpan.FromSeconds(_jwtOption.ClockSkewInSeconds),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };

            try
            {
                var a = ValidateToken(accessTokenValidationParameters, jwtToken.AccessToken);
                var b = ValidateToken(refreshTokenValidationParameters, jwtToken.RefreshToken);
                await Task.WhenAll(a, b);

                string? accessPrincipalJti = a.Result.FindFirstValue(JwtRegisteredClaimNames.Jti);
                string? refreshPrincipalJti = b.Result.FindFirstValue(JwtRegisteredClaimNames.Jti);

                if (string.IsNullOrWhiteSpace(accessPrincipalJti)
                    || string.IsNullOrWhiteSpace(refreshPrincipalJti)
                    || !refreshPrincipalJti.Equals(accessPrincipalJti, StringComparison.Ordinal))
                {
                    throw new SecurityTokenException("Tokens mismatched!");
                }

                var userId = a.Result.FindFirstValue(JwtRegisteredClaimNames.Sub);

                await tokenRepository.IsReused(refreshPrincipalJti, userId);

                return a.Result.Claims.Where(Claim => !_generatedClaims.Contains(Claim.Type)).ToList();

            }
            catch (AggregateException ex)
            {
                var messages = ex.Flatten().InnerExceptions.Select(iex => iex.Message).ToList();
                throw new SecurityTokenException(JsonSerializer.Serialize(messages));
            }
        }

        private Task<ClaimsPrincipal> ValidateToken(TokenValidationParameters tokenValidationParameters, string token)
        {
            return Task.Run(() => _jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out _));
        }
    }
}
