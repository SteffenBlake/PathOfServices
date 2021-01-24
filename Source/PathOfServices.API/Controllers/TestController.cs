using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PathOfServices.Business.Database;
using PathOfServices.Business.Services.Abstractions;

namespace PathOfServices.API.Controllers
{
    [Authorize]
    public class TestController : PathOfServicesControllerBase
    {
        private ITestEventHandler TestEventHandler { get; }

        public TestController(ITestEventHandler testEventHandler)
        {
            TestEventHandler = testEventHandler ?? throw new ArgumentNullException(nameof(testEventHandler));
        }

        [HttpPost("")]
        public async Task<IActionResult> Test()
        {
            await TestEventHandler.OnTestEventAsync();
            return Ok();
        }
    }
}
