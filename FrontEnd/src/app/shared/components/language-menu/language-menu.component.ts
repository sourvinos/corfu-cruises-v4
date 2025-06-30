import { Component } from '@angular/core'
// Custom
import { InteractionService } from '../../services/interaction.service'
import { LocalStorageService } from 'src/app/shared/services/local-storage.service'
import { MessageCalendarService } from '../../services/message-calendar.service'
import { MessageDialogService } from 'src/app/shared/services/message-dialog.service'
import { MessageInputHintService } from 'src/app/shared/services/message-input-hint.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'

@Component({
    selector: 'language-menu',
    templateUrl: './language-menu.component.html',
    styleUrls: ['./language-menu.component.css']
})

export class LanguageMenuComponent {

    //#region variables

    public feature = 'languages'

    //#endregion

    constructor(private interactionService: InteractionService, private localStorageService: LocalStorageService, private messageCalendarService: MessageCalendarService, private messageDialogService: MessageDialogService, private messageHintService: MessageInputHintService, private messageLabelService: MessageLabelService) { }

    //#region public methods

    public onChangelanguage(language: string): string {
        this.saveLanguage(language)
        this.loadMessages()
        return language
    }

    //#endregion

    //#region private methods

    private loadMessages(): void {
        this.messageCalendarService.getMessages()
        this.messageHintService.getMessages()
        this.messageLabelService.getMessages()
        this.messageDialogService.getMessages()
        this.interactionService.updateDateAdapters()
        this.interactionService.updateMenus()
        this.interactionService.updateTabTitle()
    }

    private saveLanguage(language: string): void {
        this.localStorageService.saveItem('language', language)
    }

    //#endregion

}
