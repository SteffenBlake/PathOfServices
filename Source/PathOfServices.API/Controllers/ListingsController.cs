using Microsoft.AspNetCore.Mvc;

namespace PathOfServices.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ListingsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
