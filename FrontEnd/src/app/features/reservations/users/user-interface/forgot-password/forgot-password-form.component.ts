import { Component } from '@angular/core'
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms'
import { Subject } from 'rxjs'
// Custom
import { EmailQueueDto } from 'src/app/shared/classes/email-queue-dto'
import { EmailQueueHttpService } from 'src/app/shared/services/email-queue-http.service'
import { EmojiService } from 'src/app/shared/services/emoji.service'
import { HelperService } from 'src/app/shared/services/helper.service'
import { InputTabStopDirective } from 'src/app/shared/directives/input-tabstop.directive'
import { MessageDialogService } from 'src/app/shared/services/message-dialog.service'
import { MessageInputHintService } from 'src/app/shared/services/message-input-hint.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { UserService } from '../../classes/services/user.service'
import { environment } from 'src/environments/environment'

@Component({
    selector: 'forgot-password-form',
    templateUrl: './forgot-password-form.component.html',
    styleUrls: ['../../../../../../assets/styles/custom/forms.css']
})

export class ForgotPasswordFormComponent {

    //#region 

    public feature = 'forgotPasswordForm'
    public featureIcon = 'forgot-password'
    public form: FormGroup
    public icon = 'arrow_back'
    public input: InputTabStopDirective
    public parentUrl = '/login'
    public isLoading = new Subject<boolean>()

    //#endregion

    constructor(private emailQueueHttpService: EmailQueueHttpService, private emojiService: EmojiService, private formBuilder: FormBuilder, private helperService: HelperService, private messageDialogService: MessageDialogService, private messageHintService: MessageInputHintService, private messageLabelService: MessageLabelService, private userService: UserService) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
        this.focusOnField()
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
        this.userService.getUserFromEmail(this.form.value.email).subscribe({
            next: (x) => {
                if (x.body != '') {
                    this.emailQueueHttpService.save(this.createEmailQueueObject(x)).subscribe(() => {
                        this.helperService.doPostSaveFormTasks(this.messageDialogService.emailSent(), 'ok', this.parentUrl, true)
                    })
                } else {
                    this.helperService.doPostSaveFormTasks(this.messageDialogService.emailSent(), 'ok', this.parentUrl, true)
                }
            }
        })
    }

    //#endregion

    //#region private methods

    private createEmailQueueObject(z: { body: any }): EmailQueueDto {
        return {
            initiator: 'ResetPassword',
            entityId: z.body,
            priority: 1,
            isSent: false
        }
    }

    private focusOnField(): void {
        this.helperService.focusOnField()
    }

    private initForm(): void {
        this.form = this.formBuilder.group({
            email: [environment.login.email, [Validators.required, Validators.email]]
        })
    }

    //#endregion

    //#region getters

    get email(): AbstractControl {
        return this.form.get('email')
    }

    //#endregion

}
