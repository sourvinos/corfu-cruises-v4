import { Injectable } from '@angular/core'
import { MatDialog } from '@angular/material/dialog'
import { Observable } from 'rxjs'
// Custom
import { ModalDialogComponent } from '../components/modal-dialog/modal-dialog.component'

@Injectable({ providedIn: 'root' })

export class DialogService {

    private response: any

    constructor(public dialog: MatDialog) { }

    //#region public methods

    public open(message: any, iconStyle: string, actions: string[]): Observable<boolean> {
        return this.openDialog(message, iconStyle, actions)
    }

    //#endregion

    //#region private methods

    private openDialog(message: string | object, iconStyle: string, actions: string[]): Observable<boolean> {
        this.response = this.dialog.open(ModalDialogComponent, {
            height: '25rem',
            width: '30rem',
            data: {
                message: message,
                iconStyle: iconStyle,
                actions: actions
            },
            panelClass: 'dialog'
        })
        return this.response.afterClosed()
    }

    //#endregion

}
