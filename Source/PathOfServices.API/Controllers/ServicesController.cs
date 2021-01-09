using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PathOfServices.Business.Database;

namespace PathOfServices.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServicesController : ControllerBase
    {
        private PathOfServicesDbContext DBContext { get; }
        public ServicesController(PathOfServicesDbContext dbContext)
        {
            DBContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        [HttpGet]

        public async Task<IActionResult> Index()
        {
            var data = await DBContext.Services.Select(s => new {s.Key, s.Description, category = s.Category.Name}).ToListAsync();
            return Ok(data);
        }
    }
}
