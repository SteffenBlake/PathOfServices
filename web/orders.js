const template = `
<div>
    <h1>Orders - {{ type }}</h1> 
</div>
`;

var app;
export class Orders {
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