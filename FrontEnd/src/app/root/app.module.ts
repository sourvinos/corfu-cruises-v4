// Base
import { NgModule } from '@angular/core'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { BrowserModule, Title } from '@angular/platform-browser'
import { FormsModule, ReactiveFormsModule } from '@angular/forms'
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http'
import { registerLocaleData } from '@angular/common'
// Modules
import { AppComponent } from './app.component'
import { AppRoutingModule } from './app.routing.module'
import { LoginModule } from '../features/reservations/login/classes/modules/login.module'
import { PrimeNgModule } from '../shared/modules/primeng.module'
import { SharedModule } from 'src/app/shared/modules/shared.module'
// Components
import { SalesMenuComponent } from '../shared/components/sales-menu/sales-menu.component'
import { CardsMenuComponent } from '../shared/components/home/cards-menu.component'
import { HomeComponent } from '../shared/components/home/home.component'
import { LogoutComponent } from '../shared/components/logout/logout.component'
import { ReservationSearchComponent } from '../shared/components/reservation-search/reservation-search.component'
import { ReservationSearchDialogComponent } from '../shared/components/reservation-search/reservation-search-dialog.component'
import { ReservationsMenuComponent } from '../shared/components/reservations-menu/reservations-menu.component'
import { ShortcutsMenuComponent } from '../shared/components/shortcuts-menu/shortcuts-menu.component'
import { UserMenuComponent } from '../shared/components/user-menu/user-menu.component'
// Services
import { InterceptorService } from '../shared/services/interceptor.service'
// Languages
import localeCz from '@angular/common/locales/cs'
import localeDe from '@angular/common/locales/de'
import localeEl from '@angular/common/locales/el'
import localeFr from '@angular/common/locales/fr'

registerLocaleData(localeCz)
registerLocaleData(localeDe)
registerLocaleData(localeEl)
registerLocaleData(localeFr)

@NgModule({
    declarations: [
        AppComponent,
        SalesMenuComponent,
        CardsMenuComponent,
        HomeComponent,
        LogoutComponent,
        ReservationSearchComponent,
        ShortcutsMenuComponent,
        ReservationSearchDialogComponent,
        ReservationsMenuComponent,
        UserMenuComponent
    ],
    imports: [
        AppRoutingModule,
        BrowserAnimationsModule,
        BrowserModule,
        FormsModule,
        HttpClientModule,
        LoginModule,
        PrimeNgModule,
        ReactiveFormsModule,
        SharedModule,
    ],
    providers: [
        Title, { provide: HTTP_INTERCEPTORS, useClass: InterceptorService, multi: true }
    ],
    bootstrap: [AppComponent]
})

export class AppModule { }
