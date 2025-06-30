import { enableProdMode } from '@angular/core'
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic'
// Custom
import { AppModule } from '../src/app/root/app.module'
import { environment } from './environments/environment'

if (environment.isProductionDemo || environment.isProductionLive) {
    enableProdMode()
}

platformBrowserDynamic().bootstrapModule(AppModule)
    .catch(err => console.error(err))
