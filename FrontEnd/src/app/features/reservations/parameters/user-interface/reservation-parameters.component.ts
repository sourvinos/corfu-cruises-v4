import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms'
import { ActivatedRoute, Router } from '@angular/router'
import { Component } from '@angular/core'
// Custom
import { DialogService } from 'src/app/shared/services/modal-dialog.service'
import { FormResolved } from 'src/app/shared/classes/form-resolved'
import { HelperService } from 'src/app/shared/services/helper.service'
import { InputTabStopDirective } from 'src/app/shared/directives/input-tabstop.directive'
import { MessageDialogService } from 'src/app/shared/services/message-dialog.service'
import { MessageInputHintService } from 'src/app/shared/services/message-input-hint.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { ReservationParametersHttpService } from '../classes/services/reservation-parameters-http.service'
import { ReservationParametersReadDto } from '../classes/models/reservation-parameters-read.dto'
import { ReservationParametersWriteDto } from '../classes/models/reservation-parameters-write.dto'
import { ValidationService } from 'src/app/shared/services/validation.service'

@Component({
    selector: 'reservation-parameters',
    templateUrl: './reservation-parameters.component.html',
    styleUrls: ['../../../../../assets/styles/custom/forms.css']
})

export class ReservationParametersComponent {

    //#region common

    private record: ReservationParametersReadDto
    public feature = 'reservationparameters'
    public featureIcon = 'parameters'
    public form: FormGroup
    public icon = 'arrow_back'
    public input: InputTabStopDirective
    public parentUrl = '/home'

    //#endregion

    constructor(private activatedRoute: ActivatedRoute, private dialogService: DialogService, private formBuilder: FormBuilder, private helperService: HelperService, private messageDialogService: MessageDialogService, private messageHintService: MessageInputHintService, private messageLabelService: MessageLabelService, private reservationParametersHttpService: ReservationParametersHttpService, private router: Router) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
        this.getRecord()
        this.populateFields()
        this.setSidebarsHeight()
    }

    ngAfterViewInit(): void {
        this.focusOnField()
    }

    //#endregion

    //#region public methods

    public getHint(id: string, minmax = 0): string {
        return this.messageHintService.getDescription(id, minmax)
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public onSave(): void {
        this.saveRecord(this.flattenForm())
    }

    //#endregion

    //#region private methods

    private flattenForm(): ReservationParametersWriteDto {
        return {
            id: this.form.value.id,
            closingTime: this.form.value.closingTime,
            phones: this.form.value.phones,
            email: this.form.value.email,
            linkTwistIsDemo: this.form.value.linkTwistIsDemo,
            linkTwistIsActive: this.form.value.linkTwistIsActive,
            linkTwistDemoUrl: this.form.value.linkTwistDemoUrl,
            linkTwistDemoAPIKey: this.form.value.linkTwistDemoAPIKey,
            linkTwistLiveUrl: this.form.value.linkTwistLiveUrl,
            linkTwistLiveAPIKey: this.form.value.linkTwistLiveAPIKey,
            putAt: this.form.value.putAt
        }
    }

    private focusOnField(): void {
        this.helperService.focusOnField()
    }

    private getRecord(): Promise<any> {
        return new Promise((resolve) => {
            const x = this.feature.substring(11, 22)
            const formResolved: FormResolved = this.activatedRoute.snapshot.data[x]
            if (formResolved.error == null) {
                this.record = formResolved.record.body
                resolve(this.record)
            } else {
                this.dialogService.open(this.messageDialogService.filterResponse(formResolved.error), 'error', ['ok']).subscribe(() => {
                    this.resetForm()
                    this.goBack()
                })
            }
        })
    }

    private goBack(): void {
        this.router.navigate([this.parentUrl])
    }

    private initForm(): void {
        this.form = this.formBuilder.group({
            id: [''],
            closingTime: ['00:00', [Validators.required, ValidationService.isTime]],
            phones: ['', [Validators.required, Validators.maxLength(128)]],
            email: ['', [Validators.required, Validators.maxLength(128)]],
            linkTwistIsDemo: false,
            linkTwistIsActive: false,
            linkTwistDemoUrl: ['', [Validators.maxLength(256)]],
            linkTwistDemoAPIKey: ['', [Validators.maxLength(256)]],
            linkTwistLiveUrl: ['', [Validators.maxLength(256)]],
            linkTwistLiveAPIKey: ['', [Validators.maxLength(256)]],
            postAt: [''],
            postUser: [''],
            putAt: [''],
            putUser: ['']
        })
    }

    private populateFields(): void {
        if (this.record != undefined) {
            this.form.setValue({
                id: this.record.id,
                closingTime: this.record.closingTime,
                phones: this.record.phones,
                email: this.record.email,
                linkTwistIsDemo: this.record.linkTwistIsDemo,
                linkTwistIsActive: this.record.linkTwistIsActive,
                linkTwistDemoUrl: this.record.linkTwistDemoUrl,
                linkTwistDemoAPIKey: this.record.linkTwistDemoAPIKey,
                linkTwistLiveUrl: this.record.linkTwistLiveUrl,
                linkTwistLiveAPIKey: this.record.linkTwistLiveAPIKey,
                postAt: this.record.postAt,
                postUser: this.record.postUser,
                putAt: this.record.putAt,
                putUser: this.record.putUser
            })
        }
    }

    private resetForm(): void {
        this.form.reset()
    }

    private saveRecord(parameters: ReservationParametersWriteDto): void {
        this.reservationParametersHttpService.save(parameters).subscribe({
            next: () => {
                this.helperService.doPostSaveFormTasks(this.messageDialogService.success(), 'ok', this.parentUrl, true)
            },
            error: (errorFromInterceptor) => {
                this.dialogService.open(this.messageDialogService.filterResponse(errorFromInterceptor), 'error', ['ok'])
            }
        })
    }

    private setSidebarsHeight(): void {
        this.helperService.setSidebarsTopMargin('0')
    }

    //#endregion

    //#region getters

    get closingTime(): AbstractControl {
        return this.form.get('closingTime')
    }

    get phones(): AbstractControl {
        return this.form.get('phones')
    }

    get email(): AbstractControl {
        return this.form.get('email')
    }

    get linkTwistDemoUrl(): AbstractControl {
        return this.form.get('linkTwistDemoUrl')
    }

    get linkTwistDemoAPIKey(): AbstractControl {
        return this.form.get('linkTwistDemoAPIKey')
    }

    get linkTwistLiveUrl(): AbstractControl {
        return this.form.get('linkTwistLiveUrl')
    }

    get linkTwistLiveAPIKey(): AbstractControl {
        return this.form.get('linkTwistLiveAPIKey')
    }

    get postAt(): AbstractControl {
        return this.form.get('postAt')
    }

    get postUser(): AbstractControl {
        return this.form.get('postUser')
    }

    get putAt(): AbstractControl {
        return this.form.get('putAt')
    }

    get putUser(): AbstractControl {
        return this.form.get('putUser')
    }

    //#endregion

}
