using auth.jwt.refresh_token.Entities.Base;

namespace auth.jwt.refresh_token.Abstractions.Repositories.Base
{
    public interface IRepository<TSource> where TSource : IModel
    {
        IQueryable<TSource> Query();
        Task<IEnumerable<TSource>> GetAll();
        Task<TSource?> GetById(string id);
        Task Add(TSource entity);
        Task Update(TSource entity);
        Task Delete(TSource entity);
    }
}
