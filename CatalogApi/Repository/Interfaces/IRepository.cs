using PagedList.Pagination;
using System.Linq.Expressions;

namespace CatalogApi.Repository
{
    public interface IRepository<T>
    {
        IQueryable<T> Get();
        Task<T> GetById(Expression<Func<T, bool>> predicate);
        Task<PagedList<T>> GetPaged(int pageNumber, int pageSize);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);

    }
}
