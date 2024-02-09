using auth.jwt.refresh_token.Options.Jwt.Token;
using System.ComponentModel.DataAnnotations;

namespace auth.jwt.refresh_token.Options.Jwt
{
    public sealed class JwtOption
    {
        public const string SectionName = "Jwt";

        [Required]
        public required string Audience { get; set; }

        [Required]
        public required string Issuer { get; set; }

        [Required]
        public required double ClockSkewInSeconds { get; set; }


        [Required]
        public required double NotBeforeInSeconds { get; set; }

        [Required]
        public required AccessToken AccessToken { get; set; }

        [Required]
        public required RefreshToken RefreshToken { get; set; }
    }
}
