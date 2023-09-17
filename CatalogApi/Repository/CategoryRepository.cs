using CatalogApi.Context;
using CatalogApi.Models;
using Microsoft.EntityFrameworkCore;
using PagedList.Pagination;

namespace CatalogApi.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<PagedList<Category>> GetCategory(CategoryParameters categoriaParameters)
        {
            return await PagedList<Category>.ToPagedList(Get().OrderBy(on => on.Name),
                               categoriaParameters.PageNumber,
                               categoriaParameters.PageSize);
        }

        public async Task<IEnumerable<Category>> GetProductsCategory()
        {
            return await Get().Include(x => x.Products).ToListAsync();
        }
    }
}
