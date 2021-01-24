using System.Threading;
using System.Threading.Tasks;

namespace PathOfServices.Business.Services.Abstractions
{
    public interface ITestEventHandler
    {
        Task OnTestEventAsync(CancellationToken cancellationToken = default);
    }
}
