using CatalogApi.Context;

namespace CatalogApi.Repository
{
    /// <summary>
    /// Classe UnitOfWork para agrupar as operações de repositório e garantir uma transação consistente.
    /// </summary>
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly IProductRepository _produtoRepo;
        private readonly ICategoryRepository _categoriaRepo;
        private readonly AppDbContext _dbContext;
        private readonly ILogger<UnitOfWork> _logger;

        /// <summary>
        /// Construtor para inicializar o UnitOfWork com os repositórios e contexto de banco de dados necessários.
        /// </summary>
        /// <param name="context">Contexto do banco de dados.</param>
        /// <param name="productRepository">Repositório de produtos.</param>
        /// <param name="categoryRepository">Repositório de categorias.</param>
        /// <param name="logger">Logger para registrar mensagens.</param>
        public UnitOfWork(AppDbContext context, IProductRepository productRepository, ICategoryRepository categoryRepository, ILogger<UnitOfWork> logger)
        {
            _dbContext = context;
            _produtoRepo = productRepository;
            _categoriaRepo = categoryRepository;
            _logger = logger;
        }

        /// <summary>
        /// Obtém o repositório de produtos.
        /// </summary>
        public IProductRepository ProductRepository => _produtoRepo;

        /// <summary>
        /// Obtém o repositório de categorias.
        /// </summary>
        public ICategoryRepository CategoryRepository => _categoriaRepo;

        /// <summary>
        /// Commit as alterações no banco de dados.
        /// </summary>
        /// <returns>Task para operação assíncrona.</returns>
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

        /// <summary>
        /// Libera os recursos do contexto do banco de dados.
        /// </summary>
        public void Dispose()
        {
            _dbContext?.Dispose();
        }
    }
}