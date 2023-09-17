using CatalogApi.Context;

namespace CatalogApi.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ProductRepository _produtoRepo;
        private CategoryRepository _categoriaRepo;
        public AppDbContext _dbContext;

        public UnitOfWork(AppDbContext context)
        {
            _dbContext = context;
        }

        public IProductRepository ProductRepository
        {
            get
            {
                return _produtoRepo = _produtoRepo ?? new ProductRepository(_dbContext);
            }
        }

        public ICategoryRepository CategoryRepository
        {
            get
            {
                return _categoriaRepo = _categoriaRepo ?? new CategoryRepository(_dbContext);
            }
        }

        public async Task Commit()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

    }
}
