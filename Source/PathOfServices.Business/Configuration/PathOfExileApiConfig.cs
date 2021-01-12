namespace PathOfServices.Business.Configuration
{
    public class PathOfExileApiConfig
    {
        public string Route { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public double NonceExpirySeconds { get; set; }
    }
}