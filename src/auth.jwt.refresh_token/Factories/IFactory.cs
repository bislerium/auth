namespace auth.jwt.refresh_token.Factories
{
    public interface IFactory<T>
    {
        public static abstract T Create(params object[] parameters);
    }
}
