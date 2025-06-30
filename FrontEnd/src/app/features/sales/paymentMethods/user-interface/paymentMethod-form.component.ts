import { ActivatedRoute, Router } from '@angular/router'
import { Component } from '@angular/core'
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms'
// Custom
import { DexieService } from 'src/app/shared/services/dexie.service'
import { DialogService } from 'src/app/shared/services/modal-dialog.service'
import { FormResolved } from 'src/app/shared/classes/form-resolved'
import { HelperService } from 'src/app/shared/services/helper.service'
import { InputTabStopDirective } from 'src/app/shared/directives/input-tabstop.directive'
import { MessageDialogService } from 'src/app/shared/services/message-dialog.service'
import { MessageInputHintService } from 'src/app/shared/services/message-input-hint.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { PaymentMethodHttpService } from '../classes/services/paymentMethod-http.service'
import { PaymentMethodReadDto } from '../classes/dtos/paymentMethod-read-dto'
import { PaymentMethodWriteDto } from '../classes/dtos/paymentMethod-write-dto'
import { ValidationService } from 'src/app/shared/services/validation.service'
import { environment } from 'src/environments/environment'

@Component({
    selector: 'PaymentMethod-form',
    templateUrl: './PaymentMethod-form.component.html',
    styleUrls: ['../../../../../assets/styles/custom/forms.css', './paymentMethod-form.component.css']
})

export class PaymentMethodFormComponent {

    //#region variables

    private record: PaymentMethodReadDto
    private recordId: string
    public feature = 'paymentMethodForm'
    public featureIcon = 'paymentMethods'
    public form: FormGroup
    public icon = 'arrow_back'
    public input: InputTabStopDirective
    public parentUrl = '/paymentMethods'

    //#endregion

    constructor(private activatedRoute: ActivatedRoute, private dexieService: DexieService, private dialogService: DialogService, private formBuilder: FormBuilder, private helperService: HelperService, private messageDialogService: MessageDialogService, private messageHintService: MessageInputHintService, private messageLabelService: MessageLabelService, private paymentMethodHttpService: PaymentMethodHttpService, private router: Router) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
        this.setRecordId()
        this.getRecord()
        this.populateFields()
    }

    ngAfterViewInit(): void {
        this.focusOnField()
    }

    //#endregion

    //#region public methods

    public getFlag(language: string): string {
        return environment.nationalitiesIconDirectory + language + '.png'
    }

    public getHint(id: string, minmax = 0): string {
        return this.messageHintService.getDescription(id, minmax)
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public onDelete(): void {
        this.dialogService.open(this.messageDialogService.confirmDelete(), 'question', ['abort', 'ok']).subscribe(response => {
            if (response) {
                this.paymentMethodHttpService.delete(this.form.value.id).subscribe({
                    complete: () => {
                        this.dexieService.remove('paymentMethods', this.form.value.id)
                        this.helperService.doPostSaveFormTasks(this.messageDialogService.success(), 'ok', this.parentUrl, true)
                    },
                    error: (errorFromInterceptor) => {
                        this.dialogService.open(this.messageDialogService.filterResponse(errorFromInterceptor), 'error', ['ok'])
                    }
                })
            }
        })
    }

    public onSave(): void {
        this.saveRecord(this.flattenForm())
    }

    //#endregion

    //#region private methods

    private flattenForm(): PaymentMethodWriteDto {
        return {
            id: this.form.value.id != '' ? this.form.value.id : null,
            description: this.form.value.description,
            descriptionEn: this.form.value.descriptionEn,
            myDataId: this.form.value.myDataId,
            isCash: this.form.value.isCash,
            isDefault: this.form.value.isDefault,
            isActive: this.form.value.isActive,
            putAt: this.form.value.putAt
        }
    }

    private focusOnField(): void {
        this.helperService.focusOnField()
    }

    private getRecord(): Promise<any> {
        if (this.recordId != undefined) {
            return new Promise((resolve) => {
                const formResolved: FormResolved = this.activatedRoute.snapshot.data['paymentMethodForm']
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
    }

    private goBack(): void {
        this.router.navigate([this.parentUrl])
    }

    private initForm(): void {
        this.form = this.formBuilder.group({
            id: '',
            description: ['', [Validators.required, Validators.maxLength(128)]],
            descriptionEn: ['', [Validators.required, Validators.maxLength(128)]],
            myDataId: ['', [Validators.required, Validators.maxLength(1), ValidationService.shouldBeOnlyNumbers]],
            isCash: false,
            isDefault: false,
            isActive: true,
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
                description: this.record.description,
                descriptionEn: this.record.descriptionEn,
                myDataId: this.record.myDataId,
                isCash: this.record.isCash,
                isDefault: this.record.isDefault,
                isActive: this.record.isActive,
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

    private saveRecord(paymentMethod: PaymentMethodWriteDto): void {
        this.paymentMethodHttpService.save(paymentMethod).subscribe({
            next: (response) => {
                this.dexieService.update('paymentMethods', {
                    'id': parseInt(response.id),
                    'description': paymentMethod.descriptionEn,
                    'isCash': paymentMethod.isCash,
                    'isDefault': paymentMethod.isDefault,
                    'isActive': paymentMethod.isActive
                })
                this.helperService.doPostSaveFormTasks(this.messageDialogService.success(), 'ok', this.parentUrl, true)
            },
            error: (errorFromInterceptor) => {
                this.dialogService.open(this.messageDialogService.filterResponse(errorFromInterceptor), 'error', ['ok'])
            }
        })
    }

    private setRecordId(): void {
        this.activatedRoute.params.subscribe(x => {
            this.recordId = x.id
        })
    }

    //#endregion

    //#region getters

    get description(): AbstractControl {
        return this.form.get('description')
    }

    get descriptionEn(): AbstractControl {
        return this.form.get('descriptionEn')
    }

    get myDataId(): AbstractControl {
        return this.form.get('myDataId')
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
