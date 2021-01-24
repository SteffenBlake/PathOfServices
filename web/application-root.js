import { Application } from './application.js';

export class ApplicationRoot {
    get components() {
        return {
            application: new Application()
        }
    }
}