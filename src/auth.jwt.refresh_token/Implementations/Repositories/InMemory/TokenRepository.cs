using auth.jwt.refresh_token.Abstractions.Repositories;
using auth.jwt.refresh_token.Entities;
using auth.jwt.refresh_token.Implementations.Repositories.InMemory.Base;

namespace auth.jwt.refresh_token.Implementations.Repositories.InMemory
{
    public class TokenRepository : Repository<JwtToken>, ITokenRepository
    {
        public Task<bool> IsRefreshTokenLatest(string tokenId, string userId)
        {
            return Task.Run(() =>
            {
                return _sources
                .Where(refTok => refTok.UserId.Equals(userId))
                .OrderByDescending(refTok => refTok.CreatedAtEpochTime)
                .FirstOrDefault()
                ?.Id
                .Equals(tokenId)
                ?? false;
            });
        }
    }
}
