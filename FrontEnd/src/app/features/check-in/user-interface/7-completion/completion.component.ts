import { Component } from '@angular/core'
import { Router } from '@angular/router'
// Custom
import { MessageLabelService } from 'src/app/shared/services/message-label.service'

@Component({
    selector: 'app-completion',
    templateUrl: './completion.component.html',
    styleUrls: ['./completion.component.css']
})

export class CompletionComponent {

    public feature = 'check-in'

    constructor(private messageLabelService: MessageLabelService, private router: Router) { }

    //#region public methods

    public ok(): void {
        this.router.navigateByUrl('/checkIn')
    }

    //#endregion

    //#region private methods

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    //#endregion

}
