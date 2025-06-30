import { Component } from '@angular/core'
import { MenuItem } from 'primeng/api'
// Custom
import { CryptoService } from 'src/app/shared/services/crypto.service'
import { SessionStorageService } from 'src/app/shared/services/session-storage.service'
import { environment } from './../../../../environments/environment'

@Component({
    selector: 'shortcuts-menu',
    templateUrl: './shortcuts-menu.component.html',
    styleUrls: ['./shortcuts-menu.component.css']
})

export class ShortcutsMenuComponent {

    //#region variables

    public imgIsLoaded = false
    public items: MenuItem[] | undefined

    //#endregion

    constructor(private cryptoService: CryptoService, private sessionStorageService: SessionStorageService) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.items = [
            { icon: environment.featuresIconDirectory + 'reservations' + '.svg', url: 'reservations' },
            { icon: environment.featuresIconDirectory + 'availability' + '.svg', url: 'availability' },
            { icon: environment.featuresIconDirectory + 'boarding' + '.svg', url: 'boarding' },
            { icon: environment.featuresIconDirectory + 'ledgers' + '.svg', url: 'reservation-ledgers' },
        ]
    }

    //#endregion

    //#region public methods

    public getIcon(filename: string): string {
        return environment.featuresIconDirectory + filename + '.svg'
    }

    public imageIsLoading(): any {
        return this.imgIsLoaded ? '' : 'skeleton'
    }

    public isAdmin(): boolean {
        return this.cryptoService.decrypt(this.sessionStorageService.getItem('isAdmin')) == 'true' ? true : false
    }

    public loadImage(): void {
        this.imgIsLoaded = true
    }

    //#endregion

}
