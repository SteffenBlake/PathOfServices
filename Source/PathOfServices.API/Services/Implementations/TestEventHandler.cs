using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using PathOfServices.API.Hubs;
using PathOfServices.Business.Services.Abstractions;

namespace PathOfServices.API.Services.Implementations
{
    public class TestEventHandler : ITestEventHandler
    {
        private IHubContext<TestHub> TestHubContext { get; }

        public TestEventHandler(IHubContext<TestHub> testHubContext)
        {
            TestHubContext = testHubContext ?? throw new ArgumentNullException(nameof(testHubContext));
        }

        public async Task OnTestEventAsync(CancellationToken cancellationToken = default)
        {
            await TestHubContext.Clients.All.SendAsync(TestHub.Test, cancellationToken);
        }
    }
}
