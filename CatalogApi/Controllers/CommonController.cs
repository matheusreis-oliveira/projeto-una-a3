using AutoMapper;
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
    public abstract class CommonController<TEntity, TDto> : ControllerBase
        where TEntity : BaseModel
        where TDto : class
    {
        protected readonly IUnitOfWork _uof;
        protected readonly IMapper _mapper;

        public CommonController(IUnitOfWork uof, IMapper mapper)
        {
            _uof = uof;
            _mapper = mapper;
        }

        protected abstract IRepository<TEntity> Repository { get; }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<TDto>>> Get()
        {
            var entities = await Repository.Get().ToListAsync();
            var dtos = _mapper.Map<List<TDto>>(entities);
            return dtos;
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult<TDto>> Get(Guid id)
        {
            var entity = await Repository.GetById(e => e.Id == id);

            if (entity == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<TDto>(entity);
            return dto;
        }

        [HttpPost]
        public virtual async Task<ActionResult> Post([FromBody] TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);

            Repository.Add(entity);
            await _uof.Commit();

            var newDto = _mapper.Map<TDto>(entity);

            return new CreatedAtRouteResult("GetById", new { id = entity.Id }, newDto);
        }

        [HttpPut("{id}")]
        public virtual async Task<ActionResult> Put(Guid id, [FromBody] TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);

            if (id != entity.Id)
            {
                return BadRequest();
            }

            Repository.Update(entity);
            await _uof.Commit();

            return Ok();
        }

        [HttpDelete("{id}")]
        public virtual async Task<ActionResult<TDto>> Delete(Guid id)
        {
            var entity = await Repository.GetById(e => e.Id == id);

            if (entity == null)
            {
                return NotFound();
            }

            Repository.Delete(entity);
            await _uof.Commit();

            var dto = _mapper.Map<TDto>(entity);

            return dto;
        }
    }
}