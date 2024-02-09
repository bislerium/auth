using auth.jwt.refresh_token.Abstractions.Repositories;
using auth.jwt.refresh_token.Entities;
using auth.jwt.refresh_token.Implementations.Repositories.Base;

namespace auth.jwt.refresh_token.Implementations.Repositories
{
    public class TokenRepository : InMemoryRepository<JwtToken>, ITokenRepository
    {
        public Task<bool> IsReused(string tokenId, string userId)
        {
            var result = _sources
               .Where(refTok => refTok.UserId.Equals(userId))
               .OrderByDescending(refTok => refTok.CreatedAtEpochTime)
               .FirstOrDefault()
               ?.Id
               .Equals(tokenId)
               ?? false;

            return Task.FromResult(result);
        }
    }
}
