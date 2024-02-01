using auth.jwt.refresh_token.Options.Jwt.Token;

namespace auth.jwt.refresh_token.Options.Jwt
{
    public sealed class JwtOption
    {
        public const string SectionName = "Jwt";

        public required string Audience { get; set; }

        public required string Issuer { get; set; }

        public required double ClockSkewInSeconds { get; set; }

        public required double NotBeforeInSeconds { get; set; }

        public required AccessToken AccessToken { get; set; }

        public required RefreshToken RefreshToken { get; set; }
    }
}
