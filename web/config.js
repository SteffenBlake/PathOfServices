window.config = {};

if (window.location.hostname === "127.0.0.1" || window.location.hostname === "localhost") {
    window.config.ApiEndpoint = "https://localhost:5002/api/";
    window.config.HubEndpoint = "https://localhost:5002/hubs/";
} else {
    window.config.ApiEndpoint = "https://pathof.services/api/";
    window.config.HubEndpoint = "https://pathof.services/hubs/";
}
