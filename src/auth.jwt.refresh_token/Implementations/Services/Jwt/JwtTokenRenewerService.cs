using auth.jwt.refresh_token.Abstractions.Repositories;
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
    public class JwtTokenRenewerService : IJwtTokenRenewerService
    {
        private readonly JwtOption _jwtOption;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private readonly IJwtTokenGeneratorService _jwtTokenGeneratorService;
        private readonly ITokenRepository _tokenRepository;

        public JwtTokenRenewerService(IOptions<JwtOption> jwtOption, IJwtTokenGeneratorService jwtTokenGeneratorService, ITokenRepository tokenRepository)
        {
            _jwtOption = jwtOption.Value;
            _jwtSecurityTokenHandler = new();
            _jwtSecurityTokenHandler.InboundClaimTypeMap.Clear();
            _jwtTokenGeneratorService = jwtTokenGeneratorService;
            _tokenRepository = tokenRepository;
        }

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
            return await _jwtTokenGeneratorService.GenerateJwtToken(claims);
        }

        public async Task<IEnumerable<Claim>> ValidateToken(JwtTokenDto jwtToken)
        {

            var accessTokenValidationParameters = TokenValidationParametersFactory.Create
                (
                validIssuer: _jwtOption.Issuer,
                validAudience: _jwtOption.Audience,
                issuerSigningKey: _jwtOption.AccessToken.SecurityKey,
                clockSkew: TimeSpan.FromSeconds(_jwtOption.ClockSkewInSeconds),
                validateLifetime: false
                );

            var refreshTokenValidationParameters = TokenValidationParametersFactory.Create
                (
                validIssuer: _jwtOption.Issuer,
                validAudience: _jwtOption.Audience,
                issuerSigningKey: _jwtOption.RefreshToken.SecurityKey,
                clockSkew: TimeSpan.FromSeconds(_jwtOption.ClockSkewInSeconds)
                );

            var accessTokenClaimsPricipal = ValidateToken(accessTokenValidationParameters, jwtToken.AccessToken);
            var refreshTokenClaimsPricipal = ValidateToken(refreshTokenValidationParameters, jwtToken.RefreshToken);

            string? refreshPrincipalJti = CheckAndGetJti(accessTokenClaimsPricipal, refreshTokenClaimsPricipal);
            var userId = accessTokenClaimsPricipal.FindFirstValue(JwtRegisteredClaimNames.Sub)!;
            var isRefreshTokenLatest = await _tokenRepository.IsRefreshTokenLatest(refreshPrincipalJti, userId);

            if (!isRefreshTokenLatest)
            {
                throw new SecurityTokenException("Invalid Refresh Token!");
            }

            return accessTokenClaimsPricipal
                .Claims
                .ExceptBy(_generatedClaims, x => x.Type)
                .ToList();
        }

        private static string CheckAndGetJti(ClaimsPrincipal accessTokenClaimsPrincipal, ClaimsPrincipal refreshTokenClaimsPrincipal)
        {
            string? accessTokenJti = GetJtiFromPrincipal(accessTokenClaimsPrincipal);
            string? refreshTokenJti = GetJtiFromPrincipal(refreshTokenClaimsPrincipal);

            if (string.IsNullOrWhiteSpace(accessTokenJti) || string.IsNullOrWhiteSpace(refreshTokenJti) || !string.Equals(accessTokenJti, refreshTokenJti, StringComparison.Ordinal))
            {
                throw new SecurityTokenException("Tokens mismatched!");
            }

            return refreshTokenJti;
        }

        private static string? GetJtiFromPrincipal(ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirstValue(JwtRegisteredClaimNames.Jti);
        }


        private ClaimsPrincipal ValidateToken(TokenValidationParameters tokenValidationParameters, string token)
        {            
            return _jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out _);
        }
    }
}
