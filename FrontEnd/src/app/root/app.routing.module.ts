// Base
import { NgModule } from '@angular/core'
import { NoPreloading, RouteReuseStrategy, RouterModule, Routes } from '@angular/router'
// Components
import { EmptyPageComponent } from '../shared/components/empty-page/empty-page.component'
import { ForgotPasswordFormComponent } from '../features/reservations/users/user-interface/forgot-password/forgot-password-form.component'
import { HomeComponent } from '../shared/components/home/home.component'
import { LoginFormComponent } from '../features/reservations/login/user-interface/login-form.component'
import { ResetPasswordFormComponent } from '../features/reservations/users/user-interface/reset-password/reset-password-form.component'
// Utils
import { AuthGuardService } from '../shared/services/auth-guard.service'
import { CustomRouteReuseStrategyService } from '../shared/services/route-reuse-strategy.service'

const appRoutes: Routes = [
    // Login
    { path: '', component: LoginFormComponent, pathMatch: 'full' },
    // Auth
    { path: 'login', component: LoginFormComponent },
    { path: 'forgotPassword', component: ForgotPasswordFormComponent },
    { path: 'resetPassword', component: ResetPasswordFormComponent },
    // Home
    { path: 'home', component: HomeComponent, canActivate: [AuthGuardService] },
    // Reservations
    { path: 'reservations', loadChildren: () => import('../features/reservations/reservations/classes/modules/reservation.module').then(m => m.ReservationModule) },
    { path: 'availability', loadChildren: () => import('../features/reservations/availability/classes/modules/availability.module').then(m => m.AvailabilityModule) },
    { path: 'reservation-ledgers', loadChildren: () => import('../features/reservations/ledgers/classes/modules/ledger.module').then(m => m.LedgerModule) },
    { path: 'boarding', loadChildren: () => import('../features/reservations/boarding/classes/modules/boarding.module').then(m => m.BoardingModule) },
    { path: 'manifest', loadChildren: () => import('../features/reservations/manifest/classes/modules/manifest.module').then(m => m.ManifestModule) },
    { path: 'statistics', loadChildren: () => import('../features/reservations/statistics/classes/modules/statistics.module').then(m => m.StatisticsModule) },
    { path: 'coachRoutes', loadChildren: () => import('../features/reservations/coachRoutes/classes/modules/coachRoute.module').then(m => m.CoachRouteModule) },
    { path: 'customers', loadChildren: () => import('../features/reservations/customers/classes/modules/customer.module').then(m => m.CustomerModule) },
    { path: 'destinations', loadChildren: () => import('../features/reservations/destinations/classes/modules/destination.module').then(m => m.DestinationModule) },
    { path: 'drivers', loadChildren: () => import('../features/reservations/drivers/classes/modules/driver.module').then(m => m.DriverModule) },
    { path: 'pickupPoints', loadChildren: () => import('../features/reservations/pickupPoints/classes/modules/pickupPoint.module').then(m => m.PickupPointModule) },
    { path: 'ports', loadChildren: () => import('../features/reservations/ports/classes/modules/port.module').then(m => m.PortModule) },
    { path: 'schedules', loadChildren: () => import('../features/reservations/schedules/classes/modules/schedule.module').then(m => m.ScheduleModule) },
    { path: 'shipCrews', loadChildren: () => import('../features/reservations/shipCrews/classes/modules/shipCrew.module').then(m => m.ShipCrewModule) },
    { path: 'shipOwners', loadChildren: () => import('../features/reservations/shipOwners/classes/modules/shipOwner.module').then(m => m.ShipOwnerModule) },
    { path: 'ships', loadChildren: () => import('../features/reservations/ships/classes/modules/ship.module').then(m => m.ShipModule) },
    { path: 'users', loadChildren: () => import('../features/reservations/users/classes/modules/user.module').then(m => m.UserModule) },
    { path: 'reservation-parameters', loadChildren: () => import('../features/reservations/parameters/classes/modules/reservation-parameters.module').then(m => m.ReservationParametersModule) },
    // Sales
    { path: 'banks', loadChildren: () => import('../features/sales/banks/classes/modules/bank.module').then(m => m.BankModule) },
    { path: 'bankAccounts', loadChildren: () => import('../features/sales/bankAccounts/classes/modules/bankAccount.module').then(m => m.BankAccountModule) },
    { path: 'sales-ledgers', loadChildren: () => import('../features/sales/ledgers/classes/modules/ledger-sales.module').then(m => m.LedgerSalesModule) },
    { path: 'balanceSheet', loadChildren: () => import('../features/sales/balanceSheet/classes/modules/balanceSheet.module').then(m => m.BalanceSheetModule) },
    { path: 'documentTypes', loadChildren: () => import('../features/sales/documentTypes/classes/modules/documentType.module').then(m => m.DocumentTypeModule) },
    { path: 'invoices', loadChildren: () => import('../features/sales/invoices/classes/modules/invoice.module').then(m => m.InvoiceModule) },
    { path: 'paymentMethods', loadChildren: () => import('../features/sales/paymentMethods/classes/modules/paymentMethod.module').then(m => m.PaymentMethodModule) },
    { path: 'prices', loadChildren: () => import('../features/sales/prices/classes/modules/price.module').then(m => m.PriceModule) },
    { path: 'receipts', loadChildren: () => import('../features/sales/receipts/classes/modules/receipt.module').then(m => m.ReceiptModule) },
    { path: 'taxOffices', loadChildren: () => import('../features/sales/taxOffices/classes/modules/taxOffice.module').then(m => m.TaxOfficeModule) },
    { path: 'revenues', loadChildren: () => import('../features/sales/revenues/classes/modules/revenues.module').then(m => m.RevenuesModule) },
    // CheckIn
    { path: 'checkIn', loadChildren: () => import('../features/check-in/classes/modules/check-in.module').then(m => m.CheckInModule) },
    // Empty
    { path: '**', component: EmptyPageComponent }
]

@NgModule({
    declarations: [],
    exports: [
        RouterModule
    ],
    imports: [
        RouterModule.forRoot(appRoutes, { onSameUrlNavigation: 'reload', preloadingStrategy: NoPreloading, useHash: true })
    ],
    providers: [
        { provide: RouteReuseStrategy, useClass: CustomRouteReuseStrategyService }
    ]
})

export class AppRoutingModule { }
