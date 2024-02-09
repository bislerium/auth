﻿using auth.jwt.refresh_token.Abstractions.Repositories.Base;
using auth.jwt.refresh_token.Entities;

namespace auth.jwt.refresh_token.Abstractions.Repositories
{
    public interface ITokenRepository: IRepository<JwtToken>
    {
        Task<bool> IsReused(string refreshTokenId, string userId);
    }
}