using System.ComponentModel.DataAnnotations;

namespace auth.jwt.refresh_token.Options.Jwt.Token
{
    public sealed class RefreshToken : JwtToken
    {
        [Required]
        public required double ExpirationTimeInDays { get; set; }

    }
}
