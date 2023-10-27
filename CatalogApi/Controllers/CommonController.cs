using AutoMapper;
using CatalogApi.Models;
using CatalogApi.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogApi.Controllers
{
    /// <summary>
    /// Controller abstrato comum para fornecer operações CRUD para entidades.
    /// </summary>
    /// <typeparam name="TEntity">Tipo da Entidade/Modelo</typeparam>
    /// <typeparam name="TDto">Tipo do DTO</typeparam>
    [Produces("application/json")]
    [Route("api/[Controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    
    public abstract class CommonController<TEntity, TDto> : ControllerBase
        where TEntity : BaseModel
        where TDto : class
    {
        protected readonly IUnitOfWork _uof;
        protected readonly IMapper _mapper;
        protected abstract string GetByIdRouteName { get; }

        /// <summary>
        /// Construtor para inicializar o CommonController.
        /// </summary>
        /// <param name="uof">Unit of Work.</param>
        /// <param name="mapper">AutoMapper.</param>
        public CommonController(IUnitOfWork uof, IMapper mapper)
        {
            _uof = uof;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtém o repositório associado com a entidade.
        /// </summary>
        protected abstract IRepository<TEntity> Repository { get; }

        /// <summary>
        /// Obtém todas as entidades.
        /// </summary>
        /// <returns>Uma ação que retorna uma lista de DTOs.</returns>
        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<TDto>>> Get()
        {
            var entities = await Repository.Get().ToListAsync();
            var dtos = _mapper.Map<List<TDto>>(entities);
            return dtos;
        }

        /// <summary>
        /// Obtém uma entidade pelo seu ID.
        /// </summary>
        /// <param name="id">O ID da entidade.</param>
        /// <returns>Uma ação que retorna o DTO.</returns>
        [HttpGet("{id}", Name = "GetById")]
        public virtual async Task<ActionResult<TDto>> GetById(Guid id)
        {
            var entity = await Repository.GetById(e => e.Id == id);

            if (entity == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<TDto>(entity);
            return dto;
        }

        /// <summary>
        /// Cria uma nova entidade.
        /// </summary>
        /// <param name="dto">O DTO que representa a entidade a ser criada.</param>
        /// <returns>Uma ação que retorna o resultado da criação.</returns>
        [HttpPost]
        public virtual async Task<ActionResult> Post([FromBody] TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);

            Repository.Add(entity);
            await _uof.Commit();

            var newDto = _mapper.Map<TDto>(entity);

            return CreatedAtRoute(GetByIdRouteName, new { id = entity.Id }, newDto);
        }

        /// <summary>
        /// Atualiza uma entidade existente.
        /// </summary>
        /// <param name="id">O ID da entidade a ser atualizada.</param>
        /// <param name="dto">O DTO que representa a entidade atualizada.</param>
        /// <returns>Uma ação que retorna o resultado da atualização.</returns>
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

        /// <summary>
        /// Exclui uma entidade pelo seu ID.
        /// </summary>
        /// <param name="id">O ID da entidade a ser excluída.</param>
        /// <returns>Uma ação que retorna o resultado da exclusão.</returns>
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