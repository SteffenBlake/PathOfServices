using Microsoft.EntityFrameworkCore;

namespace PathOfServices.Business.Database
{
    public class PathOfServicesDbContext : DbContext
    {
        public PathOfServicesDbContext(DbContextOptions<PathOfServicesDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ServiceCategoryEntity> Categories { get; set; }
        public DbSet<ServiceEntity> Services { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        
    }
}
