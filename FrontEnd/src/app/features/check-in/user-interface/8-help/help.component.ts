import { Component } from '@angular/core'
// Custom
import { DialogService } from 'src/app/shared/services/modal-dialog.service'
import { MessageDialogService } from 'src/app/shared/services/message-dialog.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'

@Component({
    selector: 'help',
    templateUrl: './help.component.html',
    styleUrls: ['./help.component.css']
})

export class HelpComponent {

    //#region variables

    public feature = 'check-in'

    //#endregion

    constructor(private dialogService: DialogService,private messageSnackbarService: MessageDialogService, private messageLabelService: MessageLabelService) { }

    //#region public methods

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public showHelpDialog(): void {
        this.dialogService.open(this.messageSnackbarService.helpDialog(), '', ['ok'])
    }

    //#endregion

    //#region private methods

    //#endregion

}
