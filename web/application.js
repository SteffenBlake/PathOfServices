import { Search } from './search.js';
import { Sales } from './sales.js';
import { Orders } from './orders.js';
import { Categories, Types } from './data.js';

const template = `
<nav class="navbar navbar-expand-lg navbar-dark bg-dark">
    <div class="container-fluid">
        <a class="navbar-brand" href="#">Path of Services</a>
        <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbar">
            <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbar" v-if="model.authenticated=='success'">
            <ul class="navbar-nav me-auto mb-2 mb-lg-0">
                <li class="nav-item dropdown" v-for="category in data.Categories">
                    <a class="nav-link dropdown-toggle" role="button" :id="category + '-dropdown'" data-bs-toggle="dropdown">
                        {{ category }}
                    </a>
                    <ul class="dropdown-menu dropdown-menu-dark">
                        <li v-for="type in data.Types">
                            <a class="dropdown-item" @click="navTo(category, type)">{{ type }}</a>
                        </li>
                    </ul>
                </li>
            </ul>
            <span class="navbar-text">
                <i class='bi bi-person-circle'></i> Hello {{ model.profileName }}!
            </span>
        </div>
    </div>
</nav>

<div class="container">
    <div v-if="model.authenticated=='success'">
        <search v-if="nav.category=='Sellers' || nav.category=='Buyers'" :type="nav.type" :direction='nav.category'/>
        <sales v-if="nav.category=='Sales'" :type="nav.type"/>
        <orders v-if="nav.category=='Orders'" :type="nav.type"/>
    </div>
    <div v-if="model.authenticated=='pending'" class="text-center">
        <h1>Checking Authentication...</h1>
    </div>
    <div v-if="model.authenticated=='failure'" class="text-center">
        <h1>Authentication Required</h1>
        <a :href="oauthUri()" class="btn btn-primary">Click here to authenticate!</a>
    </div>
</div>
`;

var app;
export class Application {
    get components() {
        return {
            'search': new Search(),
            'sales': new Sales(),
            'orders': new Orders()
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
            nav: {
                category: 'Sellers',
                type: 'Crafting'
            },
            data: { Categories, Types},
            model: {
                authenticated: 'pending',
                profileName: ''
            }
        }
    }

    mounted() {
        app.login();

        const location = new window.URL(window.location.href);
        if (location.searchParams.has('nav-category')) {
            app.nav.category = location.searchParams.get('nav-category');
        }
        if (location.searchParams.has('nav-type')) {
            app.nav.type = location.searchParams.get('nav-type');
        }
        app.navTo(app.nav.category, app.nav.type);
    }

    get methods() {
        return {
            oauthUri: this._oauthUri,
            login: this._login,
            loadProfile: this._loadProfile,
            navTo: this._navTo
        }
    }

    _oauthUri() {
        return window.config.ApiEndpoint + "oauth/authorize";
    }

    _login() {
        const url = window.config.ApiEndpoint + 'oauth/validate';
        window.fetch(url,
            {
                credentials: 'include'
            })
        .then(resp => resp.json())
        .then(data => {
            if (data.success) {
                app.model.authenticated = 'success';
                app.loadProfile();
            } else {
                app.model.authenticated = 'failure';
            }
        });
    }

    _loadProfile() {
        const url = window.config.ApiEndpoint + 'profile/name';
        window.fetch(url,
            {
                credentials: 'include'
            })
        .then(resp => resp.json())
        .then(data => app.model.profileName = data.userName);
    }

    _navTo(category, type) {
        app.nav.category = category;
        app.nav.type = type;
        const title = `Path of Services-${category}-${type}`;
        const location = new window.URL(window.location.href);
        location.searchParams.set('nav-category', category);
        location.searchParams.set('nav-type', type);
        window.history.pushState({ path: location.href }, title, location.href);
    }
}