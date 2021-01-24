using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PathOfServices.API.Extensions;

namespace PathOfServices.API.Hubs
{
    [Authorize]
    public class TestHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            var userName = Context.User.NameIdentifier();
            return base.OnConnectedAsync();
        }

        public static string EndPoint => "/test";

        public static string Test => "Test";
    }
}
