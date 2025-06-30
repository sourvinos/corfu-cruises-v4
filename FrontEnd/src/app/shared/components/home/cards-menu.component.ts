import { Component } from '@angular/core'
// Custom
import { CryptoService } from '../../services/crypto.service'
import { MessageLabelService } from '../../services/message-label.service'
import { Router } from '@angular/router'
import { SessionStorageService } from '../../services/session-storage.service'
import { environment } from 'src/environments/environment'

@Component({
    selector: 'cards-menu',
    templateUrl: './cards-menu.component.html',
    styleUrls: ['./cards-menu.component.css']
})

export class CardsMenuComponent {

    //#region variables

    public feature = 'cardsMenu'
    public imgIsLoaded = false

    //#endregion

    constructor(private router: Router, private cryptoService: CryptoService, private messageLabelService: MessageLabelService, private sessionStorageService: SessionStorageService) { }

    //#region public methods

    public doNavigation(feature: string, featureIsEnabled: boolean): void {
        this.router.navigate(featureIsEnabled == false
            ? (['not-ready-yet'])
            : ([feature]))
    }

    public getIcon(filename: string): string {
        return environment.featuresIconDirectory + filename + '.svg'
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
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
