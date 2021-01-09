using System.Threading.Tasks;

namespace PathOfServices.Business.Services.Abstractions
{
    public interface IDBMigrator
    {
        Task ExecuteAsync();
    }
}
