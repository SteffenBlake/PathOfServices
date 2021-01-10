window.config = {};

if (window.location.protocol === "file:") {
    window.config.ApiEndpoint = "http://localhost:5002/api/";
} else {
    window.config.ApiEndpoint = "http://pathof.services/api/";
}