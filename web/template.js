const template = `
<div>
</div>
`;

var app;
export class Application {
    get components() {
        return {

        }
    }

    beforeCreate() {
        app = this;
    }

    get template() {
        return template;
    }

    // Public
    data() {
        return {
        }
    }

    mounted() {
    }

    get methods() {
        return {
        }
    }
}