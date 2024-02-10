using auth.jwt.refresh_token.Factories.Jwt;
using auth.jwt.refresh_token.Options.Jwt;
using Microsoft.AspNetCore.Authentication;

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

                o.TokenValidationParameters = TokenValidationParametersFactory.Create
                    (
                    validIssuer: jwtOption.Issuer,
                    validAudience: jwtOption.Audience,
                    issuerSigningKey: jwtOption.AccessToken.SecurityKey,
                    clockSkew: TimeSpan.FromSeconds(jwtOption.ClockSkewInSeconds)
                    );
            });
        }
    }
}
