using CatalogApi.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CatalogApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public UserToken GenerateToken(UserDto userInfo)
        {
            // define declarações do usuário
            var claims = new[]
            {
                 new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
                 new Claim("meuPet", "pipoca"),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
             };

            // gera uma chave com base em um algoritmo simétrico
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            // gera a assinatura digital do token usando o algoritmo Hmac e a chave privada
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Tempo de expiração do token
            var expiration = DateTime.UtcNow.AddHours(double.Parse(_configuration["TokenConfiguration:ExpireHours"]));

            // classe que representa um token JWT e gera o token
            JwtSecurityToken token = new JwtSecurityToken(
              issuer: _configuration["TokenConfiguration:Issuer"],
              audience: _configuration["TokenConfiguration:Audience"],
              claims: claims,
              expires: expiration,
              signingCredentials: credentials);

            // retorna os dados com o token e informações
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
