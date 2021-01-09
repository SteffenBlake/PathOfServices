using Microsoft.Extensions.Logging;

namespace PathOfServices.Business.Configuration
{
    public class LoggingConfig
    {
        public string Path { get; set; }
        public LogLevel MinimumLevel { get; set; }
    }
}