import { Component } from '@angular/core'
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms'
import { Router } from '@angular/router'
import { Subject } from 'rxjs'
import { Title } from '@angular/platform-browser'
// Custom
import { AccountService } from '../../../../shared/services/account.service'
import { DateHelperService } from 'src/app/shared/services/date-helper.service'
import { DialogService } from 'src/app/shared/services/modal-dialog.service'
import { EmojiService } from 'src/app/shared/services/emoji.service'
import { HelperService, indicate } from 'src/app/shared/services/helper.service'
import { InputTabStopDirective } from 'src/app/shared/directives/input-tabstop.directive'
import { LocalStorageService } from 'src/app/shared/services/local-storage.service'
import { MessageDialogService } from 'src/app/shared/services/message-dialog.service'
import { MessageInputHintService } from 'src/app/shared/services/message-input-hint.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { SessionStorageService } from 'src/app/shared/services/session-storage.service'
import { environment } from 'src/environments/environment'

@Component({
    selector: 'login-form',
    templateUrl: './login-form.component.html',
    styleUrls: ['../../../../../assets/styles/custom/forms.css', './login-form.component.css']
})

export class LoginFormComponent {

    //#region common #6

    public feature = 'loginForm'
    public featureIcon = 'login'
    public form: FormGroup
    public icon = ''
    public input: InputTabStopDirective
    public parentUrl = null

    //#endregion

    //#region specific #2

    public hidePassword = true
    public isLoading = new Subject<boolean>()

    //#endregion

    constructor(
        private accountService: AccountService,
        private dateHelperService: DateHelperService,
        private dialogService: DialogService,
        private emojiService: EmojiService,
        private formBuilder: FormBuilder,
        private helperService: HelperService,
        private localStorageService: LocalStorageService,
        private messageDialogService: MessageDialogService,
        private messageHintService: MessageInputHintService,
        private messageLabelService: MessageLabelService,
        private router: Router,
        private sessionStorageService: SessionStorageService,
        private titleService: Title,
    ) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
        this.clearStoredVariables()
        this.focusOnField()
        this.setWindowTitle()
        this.checkScreenResolution()
        this.setCurrentPeriod()
        this.setNow()
        this.setSidebarsTopMargin()
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

    public onForgotPassword(): void {
        this.router.navigate(['forgotPassword'])
    }

    public onLogin(): void {
        this.accountService.login(this.form.value.username, this.form.value.password, this.localStorageService.getItem('language')).pipe(indicate(this.isLoading)).subscribe({
            complete: () => {
                this.goHome()
            },
            error: (errorFromInterceptor) => {
                this.showError(errorFromInterceptor)
            }
        })
    }

    //#endregion

    //#region private methods

    private checkScreenResolution(): void {
        if (window.screen.width < environment.minWidth || window.screen.height < 800) {
            this.dialogService.open(this.messageDialogService.resolutionWarning(), 'question', ['ok'])
        }
    }

    private clearStoredVariables(): void {
        this.accountService.clearSessionStorage()
    }

    private focusOnField(): void {
        this.helperService.focusOnField()
    }

    private goHome(): void {
        this.router.navigate(['/home'])
    }

    private initForm(): void {
        this.form = this.formBuilder.group({
            username: [environment.login.username, Validators.required],
            password: [environment.login.password, Validators.required],
            noRobot: [environment.login.noRobot, Validators.requiredTrue]
        })
    }

    private setCurrentPeriod(): void {
        this.sessionStorageService.saveItem('dayCount', this.helperService.calculateDayCount().toString())
        this.sessionStorageService.saveItem('fromDate', this.dateHelperService.getCurrentPeriodBeginDate())
        this.sessionStorageService.saveItem('toDate', this.dateHelperService.getCurrentPeriodEndDate(parseInt(this.sessionStorageService.getItem('dayCount'))))
    }

    private setNow(): void {
        this.sessionStorageService.saveItem('now', Date.now().toString())
    }

    private setSidebarsTopMargin(): void {
        this.helperService.setSidebarsTopMargin('0')
    }

    private setWindowTitle(): void {
        this.titleService.setTitle(this.helperService.getApplicationTitle())
    }

    private showError(error: any): void {
        switch (error.status) {
            case 0:
                this.dialogService.open(this.messageDialogService.noContactWithServer(), 'error', ['ok'])
                break
            case 401:
                this.dialogService.open(this.messageDialogService.authenticationFailed(), 'error', ['ok'])
                break
        }
    }

    //#endregion

    //#region getters

    get username(): AbstractControl {
        return this.form.get('username')
    }

    get password(): AbstractControl {
        return this.form.get('password')
    }

    //#endregion

}
