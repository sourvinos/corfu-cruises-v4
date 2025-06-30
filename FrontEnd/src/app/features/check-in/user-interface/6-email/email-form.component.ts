import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms'
import { Component } from '@angular/core'
import { Router } from '@angular/router'
import { Subject } from 'rxjs'
// Custom
import { CheckInHttpService } from '../../classes/services/check-in.http.service'
import { DialogService } from 'src/app/shared/services/modal-dialog.service'
import { LocalStorageService } from 'src/app/shared/services/local-storage.service'
import { MessageDialogService } from 'src/app/shared/services/message-dialog.service'
import { MessageInputHintService } from 'src/app/shared/services/message-input-hint.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'

@Component({
    selector: 'email-form',
    templateUrl: './email-form.component.html',
    styleUrls: ['./email-form.component.css']
})

export class EmailFormComponent {

    //#region variables

    public feature = 'check-in'
    public form: FormGroup
    public reservation: any
    public isLoading = new Subject<boolean>()

    //#endregion

    constructor(private messageSnackbarService: MessageDialogService, private dialogService: DialogService, private localStorageService: LocalStorageService, private checkInHttpService: CheckInHttpService, private router: Router, private formBuilder: FormBuilder, private messageLabelService: MessageLabelService, private messageHintService: MessageInputHintService) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
        this.populateForm()
    }

    //#endregion

    //#region public methods

    public finish(): void {
        this.router.navigate(['/'])
    }

    public getHint(id: string, minmax = 0): string {
        return this.messageHintService.getDescription(id, minmax)
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public previous(): void {
        this.router.navigate(['checkIn/passenger-list'])
    }

    public next(): void {
        this.reservation = JSON.parse(this.localStorageService.getItem('reservation'))
        this.reservation.email = this.form.value.email
        this.checkInHttpService.updateEmail(this.reservation).subscribe({
            complete: () => {
                this.router.navigate(['checkIn/completion'])
            },
            error: () => {
                this.dialogService.open(this.messageSnackbarService.emailNotSent(), 'error', ['ok'])
            }
        })
    }

    //#endregion

    //#region private methods

    private initForm(): void {
        this.form = this.formBuilder.group({
            email: ['', [Validators.email, Validators.maxLength(128), Validators.required]],
        })
    }

    private populateForm(): void {
        const x = JSON.parse(this.localStorageService.getItem('reservation'))
        this.form.patchValue({
            email: x.email
        })
    }

    //#endregion

    //#region getters

    get email(): AbstractControl {
        return this.form.get('email')
    }

    //#endregion

}
