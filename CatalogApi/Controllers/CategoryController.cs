using AutoMapper;
using CatalogApi.DTOs;
using CatalogApi.Models;
using CatalogApi.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagedList.Pagination;
using System.Text.Json;

namespace CatalogApi.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _context;
        private readonly IMapper _mapper;

        public CategoryController(IUnitOfWork context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("Products")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetProductsCategory()
        {
            var categories = await _context.CategoryRepository
                            .GetProductsCategory();

            var categoriesDto = _mapper.Map<List<CategoryDTO>>(categories);
            return categoriesDto;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>>
            Get([FromQuery] CategoryParameters categoriesParameters)
        {
            var category = await _context.CategoryRepository.
                                GetCategory(categoriesParameters);

            var metadata = new
            {
                category.TotalCount,
                category.PageSize,
                category.CurrentPage,
                category.TotalPages,
                category.HasNext,
                category.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));

            var categoriesDto = _mapper.Map<List<CategoryDTO>>(category);
            return categoriesDto;
        }

        [HttpGet("{id}", Name = "GetCategory")]
        public async Task<ActionResult<CategoryDTO>> Get(Guid id)
        {
            var category = await _context.CategoryRepository
                             .GetById(p => p.Id == id);

            if (category == null)
            {
                return NotFound();
            }
            var categoryDto = _mapper.Map<CategoryDTO>(category);
            return categoryDto;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CategoryDTO categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);

            _context.CategoryRepository.Add(category);
            await _context.Commit();

            var categoryDTO = _mapper.Map<CategoryDTO>(category);

            return new CreatedAtRouteResult("GetCategory",
                new { id = category.Id }, categoryDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] CategoryDTO categoryDto)
        {
            if (id != categoryDto.Id)
            {
                return BadRequest();
            }

            var category = _mapper.Map<Category>(categoryDto);

            _context.CategoryRepository.Update(category);
            await _context.Commit();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CategoryDTO>> Delete(Guid id)
        {
            var category = await _context.CategoryRepository
                            .GetById(p => p.Id == id);

            if (category == null)
            {
                return NotFound();
            }
            _context.CategoryRepository.Delete(category);
            await _context.Commit();

            var categoryDto = _mapper.Map<CategoryDTO>(category);

            return categoryDto;
        }
    }
}
