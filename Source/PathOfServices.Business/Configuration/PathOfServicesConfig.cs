using System.Collections.Generic;

namespace PathOfServices.Business.Configuration
{
    public class PathOfServicesConfig
    {
        public ConnectionType? ConnectionType { get; set; } = Configuration.ConnectionType.INVALID;
        public string ConnectionString { get; set; }
        public LoggingConfig Logging { get; set; }
        public string[] AllowedOrigins { get; set; } = new string[0];
        public IList<string> DefaultCategories { get; set; } = new List<string>();
        public IList<ServiceConfig> DefaultServices { get; set; } = new List<ServiceConfig>();
    }
}
