using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace auth.jwt.refresh_token.Options.Jwt.Token
{
    public abstract class JwtToken
    {
        public required string Key { get; set; }

        public SymmetricSecurityKey SecurityKey
        {
            get
            {
                return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
            }
        }

        public SigningCredentials SigningCredentials
        {
            get
            {
                return new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
            }
        }
    }
}
