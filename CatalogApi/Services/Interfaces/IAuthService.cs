using CatalogApi.DTOs;

namespace CatalogApi.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserToken> RegisterUser(UserDto userInfo);
        Task<UserToken> LoginUser(LoginDto userInfo);
    }
}
