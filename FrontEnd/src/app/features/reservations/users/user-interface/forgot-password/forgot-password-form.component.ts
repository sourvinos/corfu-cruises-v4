import { Component } from '@angular/core'
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms'
import { Subject } from 'rxjs'
// Custom
import { AccountService } from 'src/app/shared/services/account.service'
import { EmojiService } from 'src/app/shared/services/emoji.service'
import { HelperService, indicate } from 'src/app/shared/services/helper.service'
import { InputTabStopDirective } from 'src/app/shared/directives/input-tabstop.directive'
import { LocalStorageService } from 'src/app/shared/services/local-storage.service'
import { MessageDialogService } from 'src/app/shared/services/message-dialog.service'
import { MessageInputHintService } from 'src/app/shared/services/message-input-hint.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { environment } from 'src/environments/environment'

@Component({
    selector: 'forgot-password-form',
    templateUrl: './forgot-password-form.component.html',
    styleUrls: ['../../../../../../assets/styles/custom/forms.css']
})

export class ForgotPasswordFormComponent {

    //#region common #6

    public feature = 'forgotPasswordForm'
    public featureIcon = 'forgot-password'
    public form: FormGroup
    public icon = 'arrow_back'
    public input: InputTabStopDirective
    public parentUrl = '/login'

    //#endregion

    //#region specific #1

    public isLoading = new Subject<boolean>()

    //#endregion

    constructor(private accountService: AccountService, private emojiService: EmojiService, private formBuilder: FormBuilder, private helperService: HelperService, private localStorageService: LocalStorageService, private messageDialogService: MessageDialogService, private messageHintService: MessageInputHintService, private messageLabelService: MessageLabelService) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
        this.focusOnField()
        // this.populateFields()
    }

    //#endregion

    //#region public methods

    public getEmoji(emoji: string): string {
        return this.emojiService.getEmoji(emoji)
    }

    public getHint(id: string, minmax = 0): string {
        return this.messageHintService.getDescription(id, minmax)
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public onSubmit(): void {
        this.accountService.forgotPassword(this.form.value).pipe(indicate(this.isLoading)).subscribe({
            complete: () => {
                this.helperService.doPostSaveFormTasks(this.messageDialogService.emailSent(), 'ok', this.parentUrl, true)
            },
            error: () => {
                this.helperService.doPostSaveFormTasks(this.messageDialogService.emailNotSent(), 'error', this.parentUrl, true)
            }
        })
    }

    //#endregion

    //#region private methods

    private focusOnField(): void {
        this.helperService.focusOnField()
    }

    private initForm(): void {
        this.form = this.formBuilder.group({
            email: [environment.login.email, [Validators.required, Validators.email]]
        })
    }

    private populateFields(): void {
        this.form.patchValue({
            returnUrl: environment.clientUrl,
            language: this.localStorageService.getLanguage(),
        })
    }

    //#endregion

    //#region getters

    get email(): AbstractControl {
        return this.form.get('email')
    }

    //#endregion

}
