using auth.jwt.refresh_token.Options.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace auth.jwt.refresh_token.Configs
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
                    IssuerSigningKey = jwtOption.AccessToken.SecurityKey,
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
