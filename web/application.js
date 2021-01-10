var app;

class Application {
    beforeCreate() {
        app = this;
    }

    // Public
    data() {
        return {
            services: []
        }
    }

    mounted() {
        const url = window.config.ApiEndpoint + "services";
        window.fetch(url)
            .then(resp => resp.json())
            .then(data => {
                app.loadCategories(data.d);
            });
    }

    get methods() {
        return {
            loadCategories: this._loadCategories
        }
    }

    // Private
    _loadCategories(services) {
        app.services = services;
    }
}