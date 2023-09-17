using CatalogApi.Context;
using CatalogApi.Models;
using Microsoft.EntityFrameworkCore;
using PagedList.Pagination;

namespace CatalogApi.Repository;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<PagedList<Product>> GetProduct(ProductParameters produtosParameters)
    {

        return await PagedList<Product>.ToPagedList(Get().OrderBy(on => on.Id),
            produtosParameters.PageNumber, produtosParameters.PageSize);
    }

    public async Task<IEnumerable<Product>> GetProductsByPrice()
    {
        return await Get().OrderBy(c => c.Price).ToListAsync();
    }
}
