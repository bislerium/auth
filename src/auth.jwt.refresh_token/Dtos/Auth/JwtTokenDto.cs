using System.ComponentModel.DataAnnotations;

namespace auth.jwt.refresh_token.Dtos.Auth
{
    public class JwtTokenDto
    {
        [Required]
        public required string AccessToken { get; set; }

        [Required]
        public required string RefreshToken { get; set; }
    }
}
