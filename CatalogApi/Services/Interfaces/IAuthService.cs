using CatalogApi.DTOs;

namespace CatalogApi.Services
{
    public interface IAuthService
    {
        UserToken GenerateToken(UserDto userInfo);
    }
}
