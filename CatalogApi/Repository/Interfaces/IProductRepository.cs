using CatalogApi.Models;
using PagedList.Pagination;

namespace CatalogApi.Repository;

public interface IProductRepository : IRepository<Product>
{
    Task<PagedList<Product>> GetProduct(ProductParameters produtosParameters);
    Task<IEnumerable<Product>> GetProductsByPrice();
}
