import { Crafts, Horticrafts, Carries } from './data.js';

const template = `
<div>
    <h1>{{direction}} - {{ type }}</h1>
    <div class="mb-3">
        <label for="category" class="form-label">Category</label>
        <select id="category" class="form-select" v-model="form.category" @change="onCategoryChange()">
            <option v-for="category in Object.keys(data.services)">{{category}}</option>
        </select>
    </div>

    <div class="mb-3">
        <label for="service" class="form-label">Service</label>
        <select id="service" class="form-select" v-model="form.service" @change="onServiceChange()">
            <option v-for="service in data.services[form.category]" :value='service.shorthand'>{{service.description}}</option>
        </select>
    </div>

    <button type="submit" class="btn btn-primary" @click='onSearch'>Search</button>
</div>
`;

var app;
export class Search {
    get props() {
        return ['type', 'direction'];
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
            data: {
                services: {}
            },
            form: {
                category: '',
                service: ''
            }
        }
    }

    mounted() {
        app.load();

        const location = new window.URL(window.location.href);
        if (location.searchParams.has('nav-category') && location.searchParams.get('nav-category') === app.direction) {
            if (location.searchParams.has('form')) {
                const formStr = location.searchParams.get('form');
                app.form = JSON.parse(atob(formStr));
            }
        }
    }

    get methods() {
        return {
            load: this._load,
            onCategoryChange: this._onCategoryChange,
            onServiceChange: this._onServiceChange,
            pushHistory: this._pushHistory,
            onSearch: this._onSearch
        }
    }

    _load() {
        if (app.type === 'Crafts') {
            app.data.services = Crafts;
        } else if (app.type === 'Horticrafts') {
            app.data.services = Horticrafts;
        } else if (app.type === 'Carries') {
            app.data.services = Carries;
        }

        app.form.category = Object.keys(app.data.services)[0];
        app.form.service = app.data.services[app.form.category][0].shorthand;

        app.pushHistory();
    }

    _onCategoryChange() {
        app.form.service = app.data.services[app.form.category][0].shorthand;
        app.pushHistory();
    }

    _onServiceChange() {
        app.pushHistory();
    }

    _pushHistory() {
        const title = document.title;
        const location = new window.URL(window.location.href);
        location.searchParams.set('form', btoa(JSON.stringify(app.form)));
        window.history.pushState({ path: location.href }, title, location.href);
    }

    _onSearch() {
        
    }

    get watch() {
        return {
            type: this._load
        }
    }
}