using auth.jwt.refresh_token.Abstractions.Repositories;

namespace auth.jwt.refresh_token.Implementations.Repositories
{
    public static class TokenRepositoryInjection
    {
        public static IServiceCollection AddTokenRepository(this IServiceCollection services)
        {
            return services.AddSingleton<ITokenRepository, TokenRepository>();
        }
    }
}
