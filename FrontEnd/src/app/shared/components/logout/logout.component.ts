import { Component } from '@angular/core'
// Custom
import { AccountService } from 'src/app/shared/services/account.service'
import { DialogService } from '../../services/modal-dialog.service'
import { LocalStorageService } from 'src/app/shared/services/local-storage.service'
import { MessageDialogService } from '../../services/message-dialog.service'
import { SessionStorageService } from 'src/app/shared/services/session-storage.service'

@Component({
    selector: 'logout',
    templateUrl: './logout.component.html',
    styleUrls: ['./logout.component.css']
})

export class LogoutComponent {

    constructor(private accountService: AccountService, private dialogService: DialogService, private localStorageService: LocalStorageService, private messageDialogService: MessageDialogService, private sessionStorageService: SessionStorageService) { }

    //#region public methods

    public getIconColor(): string {
        return this.localStorageService.getItem('theme') == 'light' ? 'black' : 'white'
    }

    public isLoggedIn(): boolean {
        return this.sessionStorageService.getItem('userId') ? true : false
    }

    public onLogoutRequest(): void {
        this.dialogService.open(this.messageDialogService.confirmLogout(), 'question', ['abort', 'ok']).subscribe(response => {
            response ? this.accountService.logout() : null
        })
    }

    //#endregion

}
