using AutoMapper;
using CatalogApi.DTOs;
using CatalogApi.Models;
using CatalogApi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CatalogApi.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ProductController : CommonController<Product, ProductDTO>
    {
        public ProductController(IUnitOfWork uof, IMapper mapper)
            : base(uof, mapper)
        { }

        protected override IRepository<Product> Repository => _uof.ProductRepository;

        [HttpGet("smallprice")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductByPrice()
        {
            var product = await _uof.ProductRepository.GetProductsByPrice();
            var productsDto = _mapper.Map<List<ProductDTO>>(product);

            return productsDto;
        }
    }
}
