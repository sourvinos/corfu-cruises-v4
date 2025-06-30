import { HttpClient } from '@angular/common/http'
import { Injectable, NgZone } from '@angular/core'
import { Observable } from 'rxjs'
import { Router } from '@angular/router'
import { map } from 'rxjs/operators'
// Custom
import { BankService } from 'src/app/features/sales/banks/classes/services/bank.service'
import { ChangePasswordViewModel } from './../../features/reservations/users/classes/view-models/change-password-view-model'
import { CoachRouteService } from './../../features/reservations/coachRoutes/classes/services/coachRoute.service'
import { CrewSpecialtyHttpService } from './../../features/reservations/crewSpecialties/classes/services/crewSpecialty-http.service'
import { CryptoService } from './crypto.service'
import { CustomerHttpService } from '../../features/reservations/customers/classes/services/customer-http.service'
import { DestinationHttpService } from '../../features/reservations/destinations/classes/services/destination-http.service'
import { DexieService } from './dexie.service'
import { DocumentTypeHttpService } from 'src/app/features/sales/documentTypes/classes/services/documentType-http.service'
import { DotNetVersion } from '../classes/dotnet-version'
import { DriverService } from './../../features/reservations/drivers/classes/services/driver.service'
import { GenderService } from './../../features/reservations/genders/classes/services/gender.service'
import { HttpDataService } from './http-data.service'
import { InteractionService } from './interaction.service'
import { NationalityHttpService } from '../../features/reservations/nationalities/classes/services/nationality-http.service'
import { PaymentMethodHttpService } from 'src/app/features/sales/paymentMethods/classes/services/paymentMethod-http.service'
import { PickupPointService } from './../../features/reservations/pickupPoints/classes/services/pickupPoint.service'
import { PortHttpService } from '../../features/reservations/ports/classes/services/port-http.service'
import { ResetPasswordViewModel } from './../../features/reservations/users/classes/view-models/reset-password-view-model'
import { SessionStorageService } from './session-storage.service'
import { ShipHttpService } from '../../features/reservations/ships/classes/services/ship-http.service'
import { ShipOwnerHttpService } from '../../features/reservations/shipOwners/classes/services/shipOwner-http.service'
import { TaxOfficeService } from './../../features/sales/taxOffices/classes/services/taxOffice.service'
import { TokenRequest } from '../classes/token-request'
import { UserEmailViewModel } from 'src/app/features/reservations/users/classes/view-models/user-email-view-model'
import { environment } from '../../../environments/environment'

@Injectable({ providedIn: 'root' })

export class AccountService extends HttpDataService {

    //#region variables

    private apiUrl = environment.apiUrl
    private urlForgotPassword = this.apiUrl + '/account/patchUserWithResetEmailPending'
    private urlResetPassword = this.apiUrl + '/account/resetPassword'
    private urlToken = this.apiUrl + '/auth/auth'

    //#endregion

    constructor(httpClient: HttpClient, private bankService: BankService, private coachRouteService: CoachRouteService, private crewSpecialtyHttpService: CrewSpecialtyHttpService, private cryptoService: CryptoService, private customerHttpService: CustomerHttpService, private destinationHttpService: DestinationHttpService, private dexieService: DexieService, private documentTypeHttpService: DocumentTypeHttpService, private driverService: DriverService, private genderService: GenderService, private interactionService: InteractionService, private nationalityService: NationalityHttpService, private ngZone: NgZone, private paymentMethodService: PaymentMethodHttpService, private pickupPointService: PickupPointService, private portHttpService: PortHttpService, private router: Router, private sessionStorageService: SessionStorageService, private shipHttpService: ShipHttpService, private shipOwnerService: ShipOwnerHttpService, private taxOfficeService: TaxOfficeService) {
        super(httpClient, environment.apiUrl)
    }

    //#region public methods

    public changePassword(formData: ChangePasswordViewModel): Observable<any> {
        return this.http.post<any>(environment.apiUrl + '/account/changePassword/', formData)
    }

    public clearSessionStorage(): void {
        this.sessionStorageService.deleteItems([
            // Auth
            { 'item': 'displayName', 'when': 'always' },
            { 'item': 'expiration', 'when': 'always' },
            { 'item': 'isAdmin', 'when': 'always' },
            { 'item': 'jwt', 'when': 'always' },
            { 'item': 'now', 'when': 'always' },
            { 'item': 'refreshToken', 'when': 'always' },
            { 'item': 'returnUrl', 'when': 'always' },
            { 'item': 'userId', 'when': 'always' },
            { 'item': 'customerId', 'when': 'always' },
            { 'item': 'isFirstFieldFocused', 'when': 'always' },
            // Reservations
            { 'item': 'date', 'when': 'always' },
            { 'item': 'destination', 'when': 'always' },
            { 'item': 'fromDate', 'when': 'always' },
            { 'item': 'toDate', 'when': 'always' },
            { 'item': 'dayCount', 'when': 'always' },
            // Criteria
            { 'item': 'boarding-criteria', 'when': 'production' },
            { 'item': 'ledger-criteria', 'when': 'production' },
            { 'item': 'manifest-criteria', 'when': 'production' },
            { 'item': 'invoiceListCriteria', 'when': 'production' },
            { 'item': 'receipt-list-criteria', 'when': 'production' },
            { 'item': 'balanceSheetCriteria', 'when': 'production' },
            { 'item': 'revenuesCriteria', 'when': 'production' },
            // Tasks
            { 'item': 'reservationList-filters', 'when': 'always' }, { 'item': 'reservationList-id', 'when': 'always' }, { 'item': 'reservationList-scrollTop', 'when': 'always' },
            { 'item': 'boardingList-filters', 'when': 'always' }, { 'item': 'boardingList-id', 'when': 'always' }, { 'item': 'boardingList-scrollTop', 'when': 'always' },
            { 'item': 'ledgerList-filters', 'when': 'always' },
            // Table filters
            { 'item': 'bankList-filters', 'when': 'always' }, { 'item': 'bankList-id', 'when': 'always' }, { 'item': 'bankList-scrollTop', 'when': 'always' },
            { 'item': 'coachRouteList-filters', 'when': 'always' }, { 'item': 'coachRouteList-id', 'when': 'always' }, { 'item': 'coachRouteList-scrollTop', 'when': 'always' },
            { 'item': 'crewSpecialtyList-filters', 'when': 'always' }, { 'item': 'crewSpecialtyList-id', 'when': 'always' }, { 'item': 'crewSpecialtyList-scrollTop', 'when': 'always' },
            { 'item': 'customerList-filters', 'when': 'always' }, { 'item': 'customerList-id', 'when': 'always' }, { 'item': 'customerList-scrollTop', 'when': 'always' },
            { 'item': 'destinationList-filters', 'when': 'always' }, { 'item': 'destinationList-id', 'when': 'always' }, { 'item': 'destinationList-scrollTop', 'when': 'always' },
            { 'item': 'documentTypeList-filters', 'when': 'always' }, { 'item': 'documentTypeList-id', 'when': 'always' }, { 'item': 'documentTypeList-scrollTop', 'when': 'always' },
            { 'item': 'driverList-filters', 'when': 'always' }, { 'item': 'driverList-id', 'when': 'always' }, { 'item': 'driverList-scrollTop', 'when': 'always' },
            { 'item': 'genderList-filters', 'when': 'always' }, { 'item': 'genderList-id', 'when': 'always' }, { 'item': 'genderList-scrollTop', 'when': 'always' },
            { 'item': 'invoiceList-filters', 'when': 'always' }, { 'item': 'invoiceList-id', 'when': 'always' }, { 'item': 'invoiceList-scrollTop', 'when': 'always' },
            { 'item': 'pickupPointList-filters', 'when': 'always' }, { 'item': 'pickupPointList-id', 'when': 'always' }, { 'item': 'pickupPointList-scrollTop', 'when': 'always' },
            { 'item': 'portList-filters', 'when': 'always' }, { 'item': 'portList-id', 'when': 'always' }, { 'item': 'portList-scrollTop', 'when': 'always' },
            { 'item': 'priceList-filters', 'when': 'always' }, { 'item': 'priceList-id', 'when': 'always' }, { 'item': 'priceList-scrollTop', 'when': 'always' },
            { 'item': 'receiptList-filters', 'when': 'always' }, { 'item': 'receiptList-id', 'when': 'always' }, { 'item': 'receiptList-scrollTop', 'when': 'always' },
            { 'item': 'registrarList-filters', 'when': 'always' }, { 'item': 'registrarList-id', 'when': 'always' }, { 'item': 'registrarList-scrollTop', 'when': 'always' },
            { 'item': 'scheduleList-filters', 'when': 'always' }, { 'item': 'scheduleList-id', 'when': 'always' }, { 'item': 'scheduleList-scrollTop', 'when': 'always' },
            { 'item': 'shipCrewList-filters', 'when': 'always' }, { 'item': 'shipCrewList-id', 'when': 'always' }, { 'item': 'shipCrewList-scrollTop', 'when': 'always' },
            { 'item': 'shipList-filters', 'when': 'always' }, { 'item': 'shipList-id', 'when': 'always' }, { 'item': 'shipList-scrollTop', 'when': 'always' },
            { 'item': 'shipOwnerList-filters', 'when': 'always' }, { 'item': 'shipOwnerList-id', 'when': 'always' }, { 'item': 'shipOwnerList-scrollTop', 'when': 'always' },
            { 'item': 'shipRouteList-filters', 'when': 'always' }, { 'item': 'shipRouteList-id', 'when': 'always' }, { 'item': 'shipRouteList-scrollTop', 'when': 'always' },
            { 'item': 'userList-filters', 'when': 'always' }, { 'item': 'userList-id', 'when': 'always' }, { 'item': 'userList-scrollTop', 'when': 'always' },
            // Statistics
            { 'item': 'selectedYear', 'when': 'always' },
        ])
    }

    public forgotPassword(formData: UserEmailViewModel): Observable<any> {
        return this.http.patch<any>(environment.apiUrl + '/account/patchUserWithResetEmailPending/', formData)
    }

    public getNewRefreshToken(): Observable<any> {
        const token: TokenRequest = {
            userId: this.cryptoService.decrypt(this.sessionStorageService.getItem('userId')),
            password: null,
            grantType: 'refresh_token',
            refreshToken: sessionStorage.getItem('refreshToken'),
            language: localStorage.getItem('language')
        }
        return this.http.post<any>(this.urlToken, token).pipe(
            map(response => {
                if (response.token) {
                    this.setAuthSettings(response)
                }
                return <any>response
            })
        )
    }

    public login(userName: string, password: string, language: string): Observable<void> {
        const grantType = 'password'
        return this.http.post<any>(this.urlToken, { language, userName, password, grantType }).pipe(map(response => {
            this.setUserData(response)
            this.setDotNetVersion(response)
            this.setAuthSettings(response)
            this.populateDexieFromAPI()
            this.setSelectedYear()
            this.refreshMenus()
            this.clearConsole()
        }))
    }

    public logout(): void {
        this.clearUserTokens()
        this.refreshMenus()
        this.navigateToLogin()
        this.clearSessionStorage()
    }

    public resetPassword(vm: ResetPasswordViewModel): Observable<any> {
        return this.http.post<any>(this.urlResetPassword, vm)
    }

    //#endregion

    //#region private methods

    private clearConsole(): void {
        console.clear()
    }

    private clearUserTokens(): void {
        const userId = this.cryptoService.decrypt(this.sessionStorageService.getItem('userId'))
        this.http.post<any>(this.apiUrl + '/auth/logout', { 'userId': userId }).subscribe(() => {
            // Nothing left to do
        })
    }

    private navigateToLogin(): void {
        this.ngZone.run(() => {
            this.router.navigate(['/'])
        })
    }

    private refreshMenus(): void {
        this.interactionService.updateMenus()
    }

    private setAuthSettings(response: any): void {
        sessionStorage.setItem('expiration', response.expiration)
        sessionStorage.setItem('jwt', response.token)
        sessionStorage.setItem('refreshToken', response.refreshToken)
    }

    private populateDexieFromAPI(): void {
        // Reservations
        this.dexieService.populateNewTable('customers', this.customerHttpService)
        this.dexieService.populateTable('coachRoutes', this.coachRouteService)
        this.dexieService.populateTable('crewSpecialties', this.crewSpecialtyHttpService)
        this.dexieService.populateTable('destinations', this.destinationHttpService)
        this.dexieService.populateTable('drivers', this.driverService)
        this.dexieService.populateTable('genders', this.genderService)
        this.dexieService.populateTable('nationalities', this.nationalityService)
        this.dexieService.populateTable('pickupPoints', this.pickupPointService)
        this.dexieService.populateTable('ports', this.portHttpService)
        this.dexieService.populateTable('shipOwners', this.shipOwnerService)
        this.dexieService.populateTable('ships', this.shipHttpService)
        // Sales
        this.dexieService.populateDocumentTypesTable('documentTypesInvoice', this.documentTypeHttpService, 1)
        this.dexieService.populateDocumentTypesTable('documentTypesReceipt', this.documentTypeHttpService, 2)
        this.dexieService.populateTable('banks', this.bankService)
        this.dexieService.populateTable('paymentMethods', this.paymentMethodService)
        this.dexieService.populateTable('taxOffices', this.taxOfficeService)
        // Criteria
        this.dexieService.populateCriteria('customersCriteria', this.customerHttpService)
        this.dexieService.populateCriteria('destinationsCriteria', this.destinationHttpService)
        this.dexieService.populateCriteria('portsCriteria', this.portHttpService)
        this.dexieService.populateCriteria('shipsCriteria', this.shipHttpService)
    }

    private setDotNetVersion(response: any): void {
        DotNetVersion.version = response.dotNetVersion
    }

    private setSelectedYear(): void {
        this.sessionStorageService.saveItem('selectedYear', new Date().getFullYear().toString())
    }

    private setUserData(response: any): void {
        this.sessionStorageService.saveItem('userId', this.cryptoService.encrypt(response.userId))
        this.sessionStorageService.saveItem('displayName', this.cryptoService.encrypt(response.displayname))
        this.sessionStorageService.saveItem('isAdmin', this.cryptoService.encrypt(response.isAdmin))
        this.sessionStorageService.saveItem('customerId', this.cryptoService.encrypt(response.customerId != undefined ? response.customerId : 'null'))
        this.sessionStorageService.saveItem('isFirstFieldFocused', response.isFirstFieldFocused)
    }

    //#endregion

    //#region  getters

    get isLoggedIn(): boolean {
        if (this.sessionStorageService.getItem('userId')) {
            return true
        }
        return false
    }

    //#endregion

}
