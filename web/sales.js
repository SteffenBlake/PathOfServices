const template = `
<div>
    <h1>Sales - {{ type }}</h1>
</div>
`;

var app;
export class Sales {
    get components() {
        return {

        }
    }

    get props() {
        return ['type'];
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