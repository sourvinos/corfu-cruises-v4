import { Component, Inject } from '@angular/core'
import { Router } from '@angular/router'
// Custom
import { DestinationHttpService } from 'src/app/features/reservations/destinations/classes/services/destination-http.service'
import { GenderService } from 'src/app/features/reservations/genders/classes/services/gender.service'
import { LocalStorageService } from 'src/app/shared/services/local-storage.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { NationalityHttpService } from 'src/app/features/reservations/nationalities/classes/services/nationality-http.service'
import { DOCUMENT } from '@angular/common'

@Component({
    selector: 'app-greeting',
    templateUrl: './greeting.component.html',
    styleUrls: ['./greeting.component.css', '../../assets/checkin.css']
})

export class GreetingComponent {

    public feature = 'check-in'

    constructor(@Inject(DOCUMENT) private document: Document, private destinationService: DestinationHttpService, private genderService: GenderService, private messageLabelService: MessageLabelService, private nationalityHttpService: NationalityHttpService, private localStorageService: LocalStorageService, private router: Router) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.populateStorageFromAPI()
        this.clearLocalStorage()
        this.setLightTheme()
    }

    ngAfterViewInit(): void {
        this.removeElements()
    }

    //#endregion

    //#region public methods

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public next(): void {
        this.router.navigate(['checkIn/search'])
    }

    //#endregion

    //#region private methods

    private clearLocalStorage(): void {
        this.localStorageService.deleteItems([
            { 'item': 'criteria', 'when': 'always' },
            { 'item': 'reservation', 'when': 'always' }
        ])
    }

    private populateStorageFromAPI(): void {
        this.destinationService.getForBrowser().subscribe(response => { this.localStorageService.saveItem('destinations', JSON.stringify(response)) })
        this.genderService.getForBrowser().subscribe(response => { this.localStorageService.saveItem('genders', JSON.stringify(response)) })
        this.nationalityHttpService.getForBrowser().subscribe(response => { this.localStorageService.saveItem('nationalities', JSON.stringify(response)) })
    }

    private removeElements(): void {
        document.getElementById('topbar') ? document.getElementById('topbar').remove() : ''
        document.getElementById('leftbar') ? document.getElementById('leftbar').remove() : ''
        document.getElementById('rightbar') ? document.getElementById('rightbar').remove() : ''
    }

    private setLightTheme(): void {
        const headElement = this.document.getElementsByTagName('head')[0]
        const newLinkElement = this.document.createElement('link')
        newLinkElement.rel = 'stylesheet'
        newLinkElement.href = 'light.css'
        headElement.appendChild(newLinkElement)
    }

    //#endregion

}
