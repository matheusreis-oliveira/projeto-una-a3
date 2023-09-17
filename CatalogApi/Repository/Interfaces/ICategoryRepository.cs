using CatalogApi.Models;
using PagedList.Pagination;

namespace CatalogApi.Repository;

public interface ICategoryRepository : IRepository<Category>
{
    Task<PagedList<Category>> GetCategory(CategoryParameters categoriaParameters);
    Task<IEnumerable<Category>> GetProductsCategory();
}
