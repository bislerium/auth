using auth.jwt.refresh_token.Abstractions.Services.Jwt;

namespace auth.jwt.refresh_token.Implementations.Services.Jwt
{
    public static class JwtTokenRenewerServiceInjection
    {
        public static IServiceCollection AddJwtTokenRenewerService(this IServiceCollection services)
        {
            return services.AddSingleton<IJwtTokenRenewerService, JwtTokenRenewerService>();
        }
    }
}
