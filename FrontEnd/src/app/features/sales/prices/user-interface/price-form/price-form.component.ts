import { ActivatedRoute, Router } from '@angular/router'
import { Component } from '@angular/core'
import { DateAdapter } from '@angular/material/core'
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms'
import { Observable } from 'rxjs'
import { map, startWith } from 'rxjs/operators'
// Custom
import { DexieService } from 'src/app/shared/services/dexie.service'
import { DialogService } from '../../../../../shared/services/modal-dialog.service'
import { FormResolved } from 'src/app/shared/classes/form-resolved'
import { HelperService } from 'src/app/shared/services/helper.service'
import { InputTabStopDirective } from 'src/app/shared/directives/input-tabstop.directive'
import { InteractionService } from 'src/app/shared/services/interaction.service'
import { LocalStorageService } from 'src/app/shared/services/local-storage.service'
import { MatAutocompleteTrigger } from '@angular/material/autocomplete'
import { MessageDialogService } from 'src/app/shared/services/message-dialog.service'
import { MessageInputHintService } from 'src/app/shared/services/message-input-hint.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { PriceHttpService } from '../../classes/services/price-http.service'
import { PriceReadDto } from '../../classes/dtos/price-read-dto'
import { PriceWriteDto } from '../../classes/dtos/price-write-dto'
import { SimpleEntity } from '../../../../../shared/classes/simple-entity'
import { ValidationService } from '../../../../../shared/services/validation.service'

@Component({
    selector: 'price-form',
    templateUrl: './price-form.component.html',
    styleUrls: ['../../../../../../assets/styles/custom/forms.css']
})

export class PriceFormComponent {

    //#region common

    private record: PriceReadDto
    private recordId: number
    public feature = 'priceForm'
    public featureIcon = 'prices'
    public form: FormGroup
    public icon = 'arrow_back'
    public input: InputTabStopDirective
    public parentUrl = '/prices'

    //#endregion

    //#region autocompletes

    public isAutoCompleteDisabled = true
    public dropdownCustomers: Observable<SimpleEntity[]>
    public dropdownDestinations: Observable<SimpleEntity[]>
    public dropdownPorts: Observable<SimpleEntity[]>

    //#endregion

    constructor(private activatedRoute: ActivatedRoute, private dateAdapter: DateAdapter<any>, private dexieService: DexieService, private dialogService: DialogService, private formBuilder: FormBuilder, private helperService: HelperService, private interactionService: InteractionService, private localStorageService: LocalStorageService, private messageDialogService: MessageDialogService, private messageHintService: MessageInputHintService, private messageLabelService: MessageLabelService, private priceService: PriceHttpService, private router: Router) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
        this.setRecordId()
        this.getRecord()
        this.populateFields()
        this.populateDropdowns()
        this.subscribeToInteractionService()
        this.setLocale()
    }

    ngAfterViewInit(): void {
        this.focusOnField()
    }

    //#endregion

    //#region public methods

    public autocompleteFields(fieldName: any, object: any): any {
        return object ? object[fieldName] : undefined
    }

    public checkForEmptyAutoComplete(event: { target: { value: any } }): void {
        if (event.target.value == '') this.isAutoCompleteDisabled = true
    }

    public enableOrDisableAutoComplete(event: any): void {
        this.isAutoCompleteDisabled = this.helperService.enableOrDisableAutoComplete(event)
    }

    public formatPriceField(fieldName: string, digits: number): void {
        this.patchNumericFieldsWithZeroIfNullOrEmpty(fieldName, digits)
        this.form.patchValue({
            [fieldName]: parseFloat(this.form.value[fieldName]).toFixed(digits)
        })
    }

    public getHint(id: string, minmax = 0): string {
        return this.messageHintService.getDescription(id, minmax)
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public getRemarksLength(): any {
        return this.form.value.remarks != null ? this.form.value.remarks.length : 0
    }

    public onDelete(): void {
        this.dialogService.open(this.messageDialogService.confirmDelete(), 'question', ['abort', 'ok']).subscribe(response => {
            if (response) {
                this.priceService.delete(this.form.value.id).subscribe({
                    complete: () => {
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

    public openOrCloseAutoComplete(trigger: MatAutocompleteTrigger, element: any): void {
        this.helperService.openOrCloseAutocomplete(this.form, element, trigger)
    }

    //#endregion

    //#region private methods

    private filterAutocomplete(array: string, field: string, value: any): any[] {
        if (typeof value !== 'object') {
            const filtervalue = value.toLowerCase()
            return this[array].filter((element: { [x: string]: string }) =>
                element[field].toLowerCase().startsWith(filtervalue))
        }
    }

    private flattenForm(): PriceWriteDto {
        return {
            id: this.form.value.id != '' ? this.form.value.id : null,
            customerId: this.form.value.customer.id,
            destinationId: this.form.value.destination.id,
            portId: this.form.value.port.id,
            from: this.form.value.datePeriod.from,
            to: this.form.value.datePeriod.to,
            adultsWithTransfer: this.form.value.adultsWithTransfer,
            adultsWithoutTransfer: this.form.value.adultsWithoutTransfer,
            kidsWithTransfer: this.form.value.kidsWithTransfer,
            kidsWithoutTransfer: this.form.value.kidsWithoutTransfer,
            putAt: this.form.value.putAt
        }
    }

    private focusOnField(): void {
        this.helperService.focusOnField()
    }

    private getRecord(): Promise<any> {
        if (this.recordId != undefined) {
            return new Promise((resolve) => {
                const formResolved: FormResolved = this.activatedRoute.snapshot.data['priceForm']
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
            customer: ['', [Validators.required, ValidationService.RequireAutocomplete]],
            destination: ['', [Validators.required, ValidationService.RequireAutocomplete]],
            port: ['', [Validators.required, ValidationService.RequireAutocomplete]],
            datePeriod: this.formBuilder.group({
                from: ['', [Validators.required]],
                to: ['', [Validators.required]],
            }, {
                validator: ValidationService.validDatePeriod
            }),
            adultsWithTransfer: [0, [Validators.required, Validators.min(0), Validators.max(999.99)]],
            adultsWithoutTransfer: [0, [Validators.required, Validators.min(0), Validators.max(999.99)]],
            kidsWithTransfer: [0, [Validators.required, Validators.min(0), Validators.max(999.99)]],
            kidsWithoutTransfer: [0, [Validators.required, Validators.min(0), Validators.max(999.99)]],
            postAt: [''],
            postUser: [''],
            putAt: [''],
            putUser: ['']
        })
    }

    private patchNumericFieldsWithZeroIfNullOrEmpty(fieldName: string, digits: number): void {
        if (this.form.controls[fieldName].value == null || this.form.controls[fieldName].value == '') {
            this.form.patchValue({ [fieldName]: parseInt('0').toFixed(digits) })
        }
    }

    private populateDropdowns(): void {
        this.populateDropdownFromDexieDB('customers', 'dropdownCustomers', 'customer', 'description', 'description')
        this.populateDropdownFromDexieDB('destinations', 'dropdownDestinations', 'destination', 'description', 'description')
        this.populateDropdownFromDexieDB('ports', 'dropdownPorts', 'port', 'description', 'description')
    }

    private populateDropdownFromDexieDB(dexieTable: string, filteredTable: string, formField: string, modelProperty: string, orderBy: string): void {
        this.dexieService.table(dexieTable).orderBy(orderBy).toArray().then((response) => {
            this[dexieTable] = this.recordId == undefined ? response.filter(x => x.isActive) : response
            this[filteredTable] = this.form.get(formField).valueChanges.pipe(startWith(''), map(value => this.filterAutocomplete(dexieTable, modelProperty, value)))
        })
    }

    private populateFields(): void {
        if (this.record != undefined) {
            this.form.setValue({
                id: this.record.id,
                customer: { 'id': this.record.customer.id, 'description': this.record.customer.description },
                destination: { 'id': this.record.destination.id, 'description': this.record.destination.description },
                port: { 'id': this.record.port.id, 'description': this.record.port.description },
                datePeriod: {
                    from: this.record.from,
                    to: this.record.to,
                },
                adultsWithTransfer: this.record.adultsWithTransfer.toFixed(2),
                adultsWithoutTransfer: this.record.adultsWithoutTransfer.toFixed(2),
                kidsWithTransfer: this.record.kidsWithTransfer.toFixed(2),
                kidsWithoutTransfer: this.record.kidsWithoutTransfer.toFixed(2),
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

    private saveRecord(price: PriceWriteDto): void {
        this.priceService.save(price).subscribe({
            next: () => {
                this.helperService.doPostSaveFormTasks(this.messageDialogService.success(), 'ok', this.parentUrl, true)
            },
            error: (errorFromInterceptor: any) => {
                this.dialogService.open(this.messageDialogService.filterResponse(errorFromInterceptor), 'error', ['ok'])
            }
        })
    }

    private setLocale(): void {
        this.dateAdapter.setLocale(this.localStorageService.getLanguage())
    }

    private setRecordId(): void {
        this.activatedRoute.params.subscribe(x => {
            this.recordId = x.id
        })
    }

    private subscribeToInteractionService(): void {
        this.interactionService.refreshDateAdapter.subscribe(() => {
            this.setLocale()
        })
    }

    //#endregion

    //#region getters

    get customer(): AbstractControl {
        return this.form.get('customer')
    }

    get destination(): AbstractControl {
        return this.form.get('destination')
    }

    get port(): AbstractControl {
        return this.form.get('port')
    }

    get datePeriod(): AbstractControl {
        return this.form.get('datePeriod')
    }

    get from(): AbstractControl {
        return this.form.get('datePeriod.from')
    }

    get to(): AbstractControl {
        return this.form.get('datePeriod.to')
    }

    get adultsWithTransfer(): AbstractControl {
        return this.form.get('adultsWithTransfer')
    }

    get adultsWithoutTransfer(): AbstractControl {
        return this.form.get('adultsWithoutTransfer')
    }

    get kidsWithTransfer(): AbstractControl {
        return this.form.get('kidsWithTransfer')
    }

    get kidsWithoutTransfer(): AbstractControl {
        return this.form.get('kidsWithoutTransfer')
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
