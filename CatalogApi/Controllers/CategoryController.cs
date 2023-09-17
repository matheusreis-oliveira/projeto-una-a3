using AutoMapper;
using CatalogApi.DTOs;
using CatalogApi.Models;
using CatalogApi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CatalogApi.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CategoryController : CommonController<Category, CategoryDTO>
    {
        public CategoryController(IUnitOfWork uof, IMapper mapper)
            : base(uof, mapper)
        { }

        protected override IRepository<Category> Repository => _uof.CategoryRepository;

        [HttpGet("products")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetProductsCategory()
        {
            var categories = await _uof.CategoryRepository.GetProductsCategory();

            var categoriesDto = _mapper.Map<List<CategoryDTO>>(categories);
            return categoriesDto;
        }
    }
}
