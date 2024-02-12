using System.ComponentModel.DataAnnotations;

namespace auth.jwt.refresh_token.Options.Jwt.Token
{
    public sealed class AccessToken: JwtToken
    {

        [Required]
        public required double ExpirationTimeInMinutes { get; set; }

    }
}
