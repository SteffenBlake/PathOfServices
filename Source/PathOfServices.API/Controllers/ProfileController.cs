using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PathOfServices.API.Extensions;
using PathOfServices.API.Models.Profile;

namespace PathOfServices.API.Controllers
{
    [Authorize]
    public class ProfileController : PathOfServicesControllerBase
    {
        [HttpGet("Name")]
        public ProfileNameResult Name()
        {
            return new ProfileNameResult
            {
                UserName = User.Name()
            };
        }
    }
}
