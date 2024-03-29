﻿using auth.jwt.refresh_token.Entities.Base;

namespace auth.jwt.refresh_token.Entities
{
    public class JwtToken: IModel
    {
        public required string Id { get; set; }
        public required string UserId { get; set; }
        public required long IssuedAtEpochTime { get; set; }
        public bool Revoked { get; set; } = false;
    }
}
