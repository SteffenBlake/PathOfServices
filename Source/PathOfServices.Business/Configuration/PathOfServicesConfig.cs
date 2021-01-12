using System.Collections.Generic;

namespace PathOfServices.Business.Configuration
{
    public class PathOfServicesConfig
    {
        public ConnectionType? ConnectionType { get; set; } = Configuration.ConnectionType.INVALID;
        public string ConnectionString { get; set; }

        public PathOfExileApiConfig PathOfExileApi { get; set; }

        public LoggingConfig Logging { get; set; }
        public string Origin { get; set; }
        public List<string> DefaultCategories { get; set; } = new List<string>();
        public List<ServiceConfig> DefaultServices { get; set; } = new List<ServiceConfig>();
        public long MemoryCacheSizeLimitBytes { get; set; }
    }
}
