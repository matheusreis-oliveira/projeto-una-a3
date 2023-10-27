using CatalogApi.Context;
using CatalogApi.DTOs;
using CatalogApi.Models;
using CatalogApi.Repository;
using CatalogApi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CatalogApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly UserRepository _userRepository;
        private readonly UserManager<User> _userManager;

        public AuthService(IConfiguration configuration, AppDbContext context, UserManager<User> userManager)
        {
            _configuration = configuration;
            _userRepository = new UserRepository(context);
            _userManager = userManager;
        }

        public async Task<UserToken> RegisterUser(UserDto userInfo)
        {
            var existingUser = await _userRepository.GetById(u => u.Email == userInfo.Email);
            if (existingUser != null)
            {
                throw new Exception("Email já está em uso.");
            }

            var user = new User
            {
                Email = userInfo.Email,
                UserName = userInfo.Name,
                PasswordHash = _userManager.PasswordHasher.HashPassword(null, userInfo.Password)
            };

            _userRepository.Add(user);
            await _userRepository.SaveChangesAsync();

            return new UserToken
            {
                Authenticated = true,
                Message = "Registro bem-sucedido!"
            };
        }

        public async Task<UserToken> LoginUser(LoginDto userInfo)
        {
            var user = await _userRepository.GetById(u => u.Email == userInfo.Email);

            if (user == null)
            {
                throw new Exception("Usuário não encontrado.");
            }

            var result = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, userInfo.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new Exception("Senha inválida.");
            }

            return GenerateToken(user);
        }

        public UserToken GenerateToken(User user)
        {
            var claims = new[]
            {
                 new Claim(JwtRegisteredClaimNames.UniqueName, user.Email),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
             };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddHours(double.Parse(_configuration["TokenConfiguration:ExpireHours"]));

            JwtSecurityToken token = new JwtSecurityToken(
              issuer: _configuration["TokenConfiguration:Issuer"],
              audience: _configuration["TokenConfiguration:Audience"],
              claims: claims,
              expires: expiration,
              signingCredentials: credentials);

            return new UserToken()
            {
                Authenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
                Message = "Token JWT OK"
            };
        }
    }
}