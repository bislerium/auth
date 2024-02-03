namespace auth.jwt.refresh_token.Options.Jwt.Token
{
    public sealed class RefreshToken: JwtToken
    {
        public required double ExpirationTimeInDays { get; set; }

    }
}
