using Microsoft.AspNetCore.Mvc;

namespace PathOfServices.API.Controllers
{
    public class ListingsController : PathOfServicesControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
