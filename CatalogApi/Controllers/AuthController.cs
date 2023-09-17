using CatalogApi.DTOs;
using CatalogApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CatalogApi.Controllers
{
    /// <summary>
    /// Controller responsável pela autorização dos usuários.
    /// </summary>
    [Produces("application/json")]
    [Route("api/[Controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IAuthService _authorizationService;

        /// <summary>
        /// Construtor para inicializar o AuthorizationController com o UserManager, SignInManager e Serviço.
        /// </summary>
        /// <param name="userManager">Gerente de usuários.</param>
        /// <param name="signInManager">Gerente de login.</param>
        /// <param name="authorizationService">Serviço de autorização.</param>
        public AuthController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IAuthService authorizationService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authorizationService = authorizationService;
        }

        /// <summary>
        /// Registra um novo usuário.
        /// </summary>
        /// <param name="model">DTO contendo informações do usuário para o registro.</param>
        /// <returns>Resposta da ação de registro.</returns>
        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody] UserDto model)
        {
            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _signInManager.SignInAsync(user, false);
            return Ok(_authorizationService.GenerateToken(model));
        }

        /// <summary>
        /// Autentica um usuário.
        /// </summary>
        /// <param name="userInfo">DTO contendo informações de login do usuário.</param>
        /// <returns>Resposta da ação de login.</returns>
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UserDto userInfo)
        {
            // verifica se o modelo é válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
            }

            // verifica as credenciais do usuário e retorna um valor
            var result = await _signInManager.PasswordSignInAsync(userInfo.Email,
                userInfo.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Ok(_authorizationService.GenerateToken(userInfo));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Login Inválido....");
                return BadRequest(ModelState);
            }
        }
    }
}