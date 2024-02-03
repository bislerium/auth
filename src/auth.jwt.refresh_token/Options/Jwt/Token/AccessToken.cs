namespace auth.jwt.refresh_token.Options.Jwt.Token
{
    public sealed class AccessToken: JwtToken
    {
        public required double ExpirationTimeInMinutes { get; set; }

    }
}
