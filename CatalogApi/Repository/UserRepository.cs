using CatalogApi.Context;
using CatalogApi.Models;

namespace CatalogApi.Repository
{
    public class UserRepository : Repository<User>
    {
        public UserRepository(AppDbContext contexto) : base(contexto)
        {
        }
    }
}