using auth.jwt.refresh_token.Abstractions.Services.Jwt;

namespace auth.jwt.refresh_token.Implementations.Services.Jwt
{
    public static class JwtTokenGeneratorServiceInjection
    {
        public static IServiceCollection AddJwtTokenGeneratorService(this IServiceCollection services)
        {
            return services.AddSingleton<IJwtTokenGeneratorService, JwtTokenGeneratorService>();
        }
    }
}
