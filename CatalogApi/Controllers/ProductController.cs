using AutoMapper;
using CatalogApi.DTOs;
using CatalogApi.Models;
using CatalogApi.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogApi.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;

        public ProductController(IUnitOfWork context, IMapper mapper)
        {
            _uof = context;
            _mapper = mapper;
        }

        [HttpGet("smallprice")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductByPrice()
        {
            var product = await _uof.ProductRepository.GetProductsByPrice();
            var productsDto = _mapper.Map<List<ProductDTO>>(product);

            return productsDto;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Get()
        {
            var product = await _uof.ProductRepository.Get().ToListAsync();

            var productsDto = _mapper.Map<List<ProductDTO>>(product);
            return productsDto;
        }

        [HttpGet("{id}", Name = "GetProduct")]
        public async Task<ActionResult<ProductDTO>> Get(Guid id)
        {
            var product = await _uof.ProductRepository.GetById(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var productDto = _mapper.Map<ProductDTO>(product);
            return productDto;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProductDTO productDto)
        {
            var product = _mapper.Map<Product>(productDto);

            _uof.ProductRepository.Add(product);
            await _uof.Commit();

            var productDTO = _mapper.Map<ProductDTO>(product);

            return new CreatedAtRouteResult("GetProduct",
               new { id = product.Id }, productDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] ProductDTO productDto)
        {
            if (id != productDto.Id)
            {
                return BadRequest();
            }

            var product = _mapper.Map<Product>(productDto);

            _uof.ProductRepository.Update(product);

            await _uof.Commit();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductDTO>> Delete(Guid id)
        {
            var product = await _uof.ProductRepository.GetById(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            _uof.ProductRepository.Delete(product);
            await _uof.Commit();

            var productDto = _mapper.Map<ProductDTO>(product);

            return productDto;
        }
    }
}
