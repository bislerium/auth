using System.ComponentModel.DataAnnotations;

namespace auth.jwt.refresh_token.Dtos.Auth
{
    public sealed class LoginRequestDTO
    {
        [Required, EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
