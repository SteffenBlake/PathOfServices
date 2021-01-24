const template = `
<div v-for="service in services">
    <div>Key: {{ service.key }} </div>
    <div>Description: {{ service.description }} </div>
    <div>Category: {{ service.category }} </div>
</div>

<button type="button" @click="fireTestEvent">Test SignalR!</button>

<div v-for="testEvent in testEvents">
    <span>{{ testEvent }}</span>
</div>

`;

var app;
export class Application {

    beforeCreate() {
        app = this;
    }

    get template() {
        return template;
    }

    // Public
    data() {
        return {
            count: 0,
            services: [],
            testEvents: [],
            connection: null
        }
    }

    mounted() {
        app.connection = new window.signalR.HubConnectionBuilder().withUrl(window.config.HubEndpoint + "test").build();
        app.connection.on("Test", app.onTestEvent);
        app.connection.start();

        const url = window.config.ApiEndpoint + "services";
        window.fetch(url,
                {
                    credentials: 'include'
                })
            .then(resp => resp.json())
            .then(data => {
                app.loadCategories(data.d);
            });
    }

    get methods() {
        return {
            loadCategories: this._loadCategories,
            onTestEvent: this._onTestEvent,
            fireTestEvent: this._fireTestEvent,
        }
    }

    // Private
    _loadCategories(services) {
        app.services = services;
    }

    _fireTestEvent() {
        const url = window.config.ApiEndpoint + "test";
        window.fetch(url,
            {
                method: 'POST',
                credentials: 'include'
            });
    }

    _onTestEvent() {
        app.testEvents.push("Event heard!");
    }
}