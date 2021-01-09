using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PathOfServices.Business.Configuration;
using PathOfServices.Business.Database;
using PathOfServices.Business.Services.Abstractions;

namespace PathOfServices.Business.Services.Implementations
{
    public class DBMigrator : IDBMigrator
    {
        private PathOfServicesDbContext DBContext { get; }
        private PathOfServicesConfig Config { get; }

        public DBMigrator(PathOfServicesDbContext dbContext, PathOfServicesConfig config)
        {
            DBContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            Config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task ExecuteAsync()
        {
            await DBContext.Database.MigrateAsync();

            await BuildDefaultCategories();

            await BuildDefaultServices();

        }

        private async Task BuildDefaultCategories()
        {
            var categoryNames = await DBContext.Categories.Select(c => c.Name).ToListAsync();
            foreach (var name in Config.DefaultCategories.Except(categoryNames))
            {
                await DBContext.AddAsync(new ServiceCategoryEntity()
                {
                    Name = name
                });
            }

            await DBContext.SaveChangesAsync();
        }

        private async Task BuildDefaultServices()
        {
            var categories = await DBContext.Categories.ToDictionaryAsync(c => c.Name, c => c.Id);

            foreach (var serviceConfig in Config.DefaultServices)
            {
                var match = await DBContext.Services.Include(s => s.Category).SingleOrDefaultAsync(s => s.Key == serviceConfig.Key);

                // Same key and Description, skip
                if (match != null && match.Description == serviceConfig.Description && match.Category.Name == serviceConfig.Category)
                {
                    continue;
                }

                // Same key but new description, update description
                if (match != null)
                {
                    match.Description = serviceConfig.Description;
                    match.CategoryId = categories[serviceConfig.Category];
                    DBContext.Update(match);
                    continue;
                }

                // New entry, add to DB
                await DBContext.AddAsync(new ServiceEntity
                {
                    Key = serviceConfig.Key,
                    Description = serviceConfig.Description,
                    CategoryId = categories[serviceConfig.Category]
                });
            }

            await DBContext.SaveChangesAsync();
        }
    }
}