using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace PathOfServices.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class PathOfServicesControllerBase : ControllerBase
    {
        /// <summary>
        /// Override method for OK Result that wraps json array in a 'd' Json object to prevent security vulnerability
        /// </summary>
        [NonAction]
        public virtual OkObjectResult Ok<T>(IEnumerable<T> d)
        {
            return Ok(new {d});
        }
    }
}
