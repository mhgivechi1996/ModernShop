using Identity.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Data
{
    public class IdentityDbContext : DbContext
    {
        public IdentityDbContext(DbContextOptions options):base(options)
        {

        }

        public DbSet<User> Users => Set<User>();
        public DbSet<UserRefreshToken> UserRefreshTokens => Set<UserRefreshToken>();
    }
}
