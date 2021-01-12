using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PathOfServices.Business.Database
{
    public class PathOfServicesDbContext : IdentityDbContext
    {
        public PathOfServicesDbContext(DbContextOptions<PathOfServicesDbContext> options)
            : base(options)
        {
        }

        // Authorization
        public DbSet<OAuthCodeEntity> OAuthCodes { get; set; }
        public DbSet<OAuthTokenEntity> OAuthTokens { get; set; }

        // Functionality
        public DbSet<ServiceCategoryEntity> Categories { get; set; }
        public DbSet<ServiceEntity> Services { get; set; }
        public DbSet<SaleEntity> Sales { get; set; }
        public DbSet<ReputationEntity> Reputation { get; set; }
    }
}
