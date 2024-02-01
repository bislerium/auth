using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace auth.jwt.non_refresh_token.Options
{
    public sealed class JwtOption
    {
        public const string SectionName = "Jwt";

        public required string Audience { get; set; }

        public required string Issuer { get; set; }

        public required string Key { get; set; }

        public double ClockSkewInSeconds { get; set; }

        public double NotBeforeInSeconds { get; set; }

        public double ExpirationTimeInMinutes { get; set; }

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
