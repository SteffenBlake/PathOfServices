const template = `

<nav class="navbar navbar-expand-lg navbar-dark bg-dark">
    <div class="container-fluid">
        <a class="navbar-brand" href="#">Path of Services</a>
        <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbar">
            <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbar" v-if="authenticated=='success'">
            <ul class="navbar-nav me-auto mb-2 mb-lg-0">
                <li class="nav-item">
                    <a class="nav-link" :class="{ active: nav=='Sellers' }" @click="nav='Sellers'">Sellers</a>
                </li>
                <li class="nav-item" role="presentation">
                    <a class="nav-link" :class="{ active: nav=='Buyers' }" @click="nav='Buyers'">Buyers</a>
                </li>
                <li class="nav-item" role="presentation">
                    <a class="nav-link" :class="{ active: nav=='Sell' }" @click="nav='Sell'">Sell</a>
                </li>
                <li class="nav-item" role="presentation">
                    <a class="nav-link" :class="{ active: nav=='Buy' }" @click="nav='Buy'">Buy</a>
                </li>
            </ul>
            <span class="navbar-text">
                <i class='bi bi-person-circle'></i> Hello {{ profileName }}!
            </span>
        </div>
    </div>
</nav>

<div class="container">
    <div v-if="authenticated=='success'">
        <div v-if="nav=='Sellers'">
            Sellers
        </div>
        <div v-if="nav=='Buyers'">
            Buyers
        </div>
        <div v-if="nav=='Sell'">
            Sell
        </div>
        <div v-if="nav=='Buy'">
            Buy
        </div>
    </div>
    <div v-if="authenticated=='pending'" class="text-center">
        <h1>Checking Authentication...</h1>
    </div>
    <div v-if="authenticated=='failure'" class="text-center">
        <h1>Authentication Required</h1>
        <a :href="oauthUri()" class="btn btn-primary">Click here to authenticate!</a>
    </div>
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
            nav: 'Sellers',
            authenticated: 'pending',
            profileName: ''
        }
    }

    mounted() {
        app.login();
    }

    get methods() {
        return {
            oauthUri: this._oauthUri,
            login: this._login,
            loadProfile: this._loadProfile
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
                app.authenticated = 'success';
                app.loadProfile();
            } else {
                app.authenticated = 'failure';
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
        .then(data => app.profileName = data.userName);
    }
}