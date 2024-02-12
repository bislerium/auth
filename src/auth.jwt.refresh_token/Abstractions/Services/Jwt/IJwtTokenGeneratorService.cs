using auth.jwt.refresh_token.Dtos.Auth;
using System.Security.Claims;

namespace auth.jwt.refresh_token.Abstractions.Services.Jwt
{
    public interface IJwtTokenGeneratorService
    {
        Task<JwtTokenDto> GenerateJwtToken(IEnumerable<Claim> claims);
    }
}
