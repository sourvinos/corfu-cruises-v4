import { Component, Input } from '@angular/core'
// Custom
import { environment } from 'src/environments/environment'

@Component({
    selector: 'cat-page',
    templateUrl: './cat-page.component.html',
    styleUrls: ['./cat-page.component.css']
})

export class CatPageComponent {

    //#region variables

    @Input() header: string
    @Input() subHeader: string
    @Input() icon: string

    public imgIsLoaded = false

    //#endregion

    //#region public methods

    public getIcon(filename: string): string {
        return environment.featuresIconDirectory + filename + '.svg'
    }

    public imageIsLoading(): any {
        return this.imgIsLoaded ? '' : 'skeleton'
    }

    public loadImage(): void {
        this.imgIsLoaded = true
    }

    //#endregion

}
