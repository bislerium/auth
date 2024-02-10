using auth.jwt.refresh_token.Abstractions.Repositories;

namespace auth.jwt.refresh_token.Implementations.Repositories.InMemory
{
    public static class TokenRepositoryInjection
    {
        public static IServiceCollection AddInMemoryTokenRepository(this IServiceCollection services)
        {
            return services.AddSingleton<ITokenRepository, TokenRepository>();
        }
    }
}
