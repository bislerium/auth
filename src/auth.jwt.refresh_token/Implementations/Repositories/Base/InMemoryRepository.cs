using auth.jwt.refresh_token.Abstractions.Repositories.Base;
using auth.jwt.refresh_token.Entities.Base;

namespace auth.jwt.refresh_token.Implementations.Repositories.Base
{
    public class InMemoryRepository<TSource> : IRepository<TSource> where TSource : IModel
    {
        protected readonly List<TSource> _sources = [];

        public IQueryable<TSource> Query() => _sources.AsQueryable();

        public Task<IEnumerable<TSource>> GetAll() => Task.FromResult(_sources.AsEnumerable());

        public Task<TSource?> GetById(string id) => Task.FromResult(_sources.FirstOrDefault(x => x.Id.Equals(id)));

        public Task Add(TSource entity)
        {
            _sources.Add(entity);
            return Task.CompletedTask;
        }

        public Task Update(TSource entity)
        {
            var index = _sources.FindIndex(p => p.Id.Equals(entity.Id));

            if (index != -1)
            {
                _sources[index] = entity;
            }
            else
            {
                throw new InvalidOperationException($"{nameof(TSource)} with ID {entity.Id} not found.");
            }
            return Task.CompletedTask;
        }

        public Task Delete(TSource entity)
        {
            _sources.Remove(entity);
            return Task.CompletedTask;
        }
    }
}
