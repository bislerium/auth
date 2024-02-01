using auth.jwt.non_refresh_token.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace auth.jwt.non_refresh_token.Configs
{
    public static class JwtBearerConfig
    {
        public static AuthenticationBuilder AddCustomJwtBearer(this AuthenticationBuilder builder, IConfiguration configuration)
        {
            return builder.AddJwtBearer(o =>
            {
                var jwtOption = configuration
                    .GetRequiredSection(JwtOption.SectionName)
                    .Get<JwtOption>()
                    ?? throw new InvalidOperationException($"Cannot bind to {nameof(JwtOption)}!");

                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwtOption.Issuer,
                    ValidAudience = jwtOption.Audience,
                    IssuerSigningKey = jwtOption.SecurityKey,
                    ClockSkew = TimeSpan.FromSeconds(jwtOption.ClockSkewInSeconds),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });
        }
    }
}
