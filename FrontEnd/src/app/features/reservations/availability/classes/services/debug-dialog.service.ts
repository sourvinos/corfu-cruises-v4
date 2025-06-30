import { Injectable } from '@angular/core'
import { MatDialog } from '@angular/material/dialog'
import { Observable } from 'rxjs'
// Custom
import { ModalDebugDialogComponent } from '../../user-interface/modal-debug-dialog/modal-debug-dialog.component'

@Injectable({ providedIn: 'root' })

export class DebugDialogService {

    private response: any

    constructor(public dialog: MatDialog) { }

    //#region public methods

    public open(message: any, iconStyle: string, actions: string[]): Observable<boolean> {
        return this.openDialog(message, iconStyle, actions)
    }

    //#endregion

    //#region private methods

    private openDialog(message: string | object, iconStyle: string, actions: string[]): Observable<boolean> {
        this.response = this.dialog.open(ModalDebugDialogComponent, {
            height: '30rem',
            width: '31rem',
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
