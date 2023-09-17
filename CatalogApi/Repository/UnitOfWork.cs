using CatalogApi.Context;

namespace CatalogApi.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly IProductRepository _produtoRepo;
        private readonly ICategoryRepository _categoriaRepo;
        private readonly AppDbContext _dbContext;
        private readonly ILogger<UnitOfWork> _logger;

        public UnitOfWork(AppDbContext context, IProductRepository productRepository, ICategoryRepository categoryRepository, ILogger<UnitOfWork> logger)
        {
            _dbContext = context;
            _produtoRepo = productRepository;
            _categoriaRepo = categoryRepository;
            _logger = logger;
        }

        public IProductRepository ProductRepository => _produtoRepo;

        public ICategoryRepository CategoryRepository => _categoriaRepo;

        public async Task Commit()
        {
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao salvar as mudanças no banco de dados.");
                throw;
            }
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }
    }
}