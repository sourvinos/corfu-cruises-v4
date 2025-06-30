import { ChangeDetectorRef, Component, HostListener } from '@angular/core'
import { NavigationCancel, NavigationEnd, NavigationError, NavigationStart, Router } from '@angular/router'
// Custom
import { AccountService } from '../shared/services/account.service'
import { CryptoService } from '../shared/services/crypto.service'
import { LoadingSpinnerService } from '../shared/services/loading-spinner.service'
import { SessionStorageService } from '../shared/services/session-storage.service'
import { environment } from 'src/environments/environment'
import { routeAnimation } from '../shared/animations/animations'

@Component({
    selector: 'root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css'],
    animations: [routeAnimation]
})

export class AppComponent {

    //#region variables

    public isLoading = true

    //#endregion

    constructor(private accountService: AccountService, private changeDetector: ChangeDetectorRef, private cryptoService: CryptoService, private loadingSpinnerService: LoadingSpinnerService, private router: Router, private sessionStorageService: SessionStorageService) {
        this.router.events.subscribe((routerEvent) => {
            if (routerEvent instanceof NavigationStart) {
                this.isLoading = true
            }
            if (routerEvent instanceof NavigationEnd || routerEvent instanceof NavigationCancel || routerEvent instanceof NavigationError) {
                this.isLoading = false
            }
        })
    }

    //#region listeners

    @HostListener('window:beforeunload', ['$event']) beforeUnloadHander(): any {
        this.accountService.logout()
    }

    //#endregion

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initLoadingSpinner()
        this.setUserSelect()
        this.setBackgroundImage()
        this.openBroadcastChannel()
        this.isUserConnected()
    }

    //#endregion

    //#region private methods

    private initLoadingSpinner(): void {
        this.loadingSpinnerService.getSpinnerObserver().subscribe((status) => {
            this.isLoading = status == 'start'
            this.changeDetector.detectChanges()
        })
    }

    private isUserConnected(): void {
        if (this.cryptoService.decrypt(this.sessionStorageService.getItem('userId')) != '' && window.location.href.includes('resetPassword') == false) {
            this.accountService.logout()
        }
    }

    private openBroadcastChannel(): void {
        new BroadcastChannel('test').postMessage('open')
    }

    private setBackgroundImage(): void {
        document.getElementById('wrapper').style.backgroundImage = 'url(../../assets/images/themes/background.svg'
    }

    private setUserSelect(): void {
        document.getElementById('main').style.userSelect = environment.cssUserSelect
    }

    //#endregion

}
