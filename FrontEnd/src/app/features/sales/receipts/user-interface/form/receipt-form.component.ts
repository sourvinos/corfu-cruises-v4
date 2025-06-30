import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms'
import { ActivatedRoute, Router } from '@angular/router'
import { Component } from '@angular/core'
import { MatAutocompleteTrigger } from '@angular/material/autocomplete'
import { Observable, map, startWith } from 'rxjs'
// Custom
import { DebugDialogService } from 'src/app/features/reservations/availability/classes/services/debug-dialog.service'
import { DexieService } from 'src/app/shared/services/dexie.service'
import { DialogService } from 'src/app/shared/services/modal-dialog.service'
import { DocumentTypeAutoCompleteVM } from '../../../documentTypes/classes/view-models/documentType-autocomplete-vm'
import { DocumentTypeHttpService } from '../../../documentTypes/classes/services/documentType-http.service'
import { DocumentTypeReadDto } from '../../../documentTypes/classes/dtos/documentType-read-dto'
import { FormResolved } from 'src/app/shared/classes/form-resolved'
import { HelperService } from 'src/app/shared/services/helper.service'
import { InputTabStopDirective } from 'src/app/shared/directives/input-tabstop.directive'
import { MessageDialogService } from 'src/app/shared/services/message-dialog.service'
import { MessageInputHintService } from 'src/app/shared/services/message-input-hint.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { ReceiptHelperService } from '../../classes/services/receipt.helper.service'
import { ReceiptHttpService } from '../../classes/services/receipt-http.service'
import { ReceiptReadDto } from '../../classes/dtos/receipt-read-dto'
import { ReceiptWriteDto } from '../../classes/dtos/receipt-write-dto'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'
import { ValidationService } from 'src/app/shared/services/validation.service'

@Component({
    selector: 'receipt-form',
    templateUrl: './receipt-form.component.html',
    styleUrls: ['../../../../../../assets/styles/custom/forms.css', './receipt-form.component.css']
})

export class ReceiptFormComponent {

    //#region common variables

    private record: ReceiptReadDto
    private recordId: number
    public feature = 'receiptForm'
    public featureIcon = 'receipts'
    public form: FormGroup
    public icon = 'arrow_back'
    public input: InputTabStopDirective
    public parentUrl = '/receipts'

    //#endregion

    //#region autocompletes

    public isAutoCompleteDisabled = true
    public dropdownCustomers: Observable<SimpleEntity[]>
    public dropdownDocumentTypes: Observable<DocumentTypeAutoCompleteVM[]>
    public dropdownPaymentMethods: Observable<SimpleEntity[]>
    public dropdownShipOwners: Observable<SimpleEntity[]>

    //#endregion

    constructor(private activatedRoute: ActivatedRoute, private debugDialogService: DebugDialogService, private dexieService: DexieService, private dialogService: DialogService, private documentTypeHttpService: DocumentTypeHttpService, private formBuilder: FormBuilder, private helperService: HelperService, private messageDialogService: MessageDialogService, private messageHintService: MessageInputHintService, private messageLabelService: MessageLabelService, private receiptHelperService: ReceiptHelperService, private receiptHttpService: ReceiptHttpService, private router: Router) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
        this.setRecordId()
        this.getRecord()
        this.populateFields()
        this.populateDropdowns()
        this.populateDocumentTypes()
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

    public getHint(id: string, minmax = 0): string {
        return this.messageHintService.getDescription(id, minmax)
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public isEditingAllowed(): boolean {
        return this.recordId == undefined
    }

    public isCancelled(): boolean {
        return this.form.value.isCancelled
    }

    public isEmailSent(): boolean {
        return this.form.value.isEmailSent
    }

    public isSubmitted(): boolean {
        return false
    }

    public onCreateAndOpenPdf(): void {
        this.receiptHttpService.buildPdf(new Array(this.form.value.invoiceId)).subscribe({
            next: (response) => {
                this.receiptHttpService.openPdf(response.body[0]).subscribe({
                    next: (response) => {
                        const blob = new Blob([response], { type: 'application/pdf' })
                        const fileURL = URL.createObjectURL(blob)
                        window.open(fileURL, '_blank')
                    },
                    error: (errorFromInterceptor) => {
                        this.dialogService.open(this.messageDialogService.filterResponse(errorFromInterceptor), 'error', ['ok'])
                    }
                })
            },
            error: (errorFromInterceptor) => {
                this.dialogService.open(this.messageDialogService.filterResponse(errorFromInterceptor), 'error', ['ok'])
            }
        })
    }

    public onCancelInvoice(): void {
        this.dialogService.open(this.messageDialogService.confirmCancelInvoice(), 'question', ['abort', 'ok']).subscribe(response => {
            if (response) {
                this.receiptHttpService.patchInvoiceWithIsCancelled(this.form.value.invoiceId).subscribe({
                    next: () => {
                        this.form.patchValue({
                            isCancelled: true
                        })
                        this.helperService.doPostSaveFormTasks(this.messageDialogService.success(), 'ok', this.parentUrl, false)
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

    public onShowFormValue(): void {
        this.debugDialogService.open(this.form.value, '', ['ok'])
    }

    public updateFieldsAfterShipOwnerSelection(value: SimpleEntity): void {
        this.form.patchValue({
            documentType: '',
            documentTypeDescription: '',
            invoiceNo: 0,
            batch: ''
        })
        this.populateDocumentTypesAfterShipOwnerSelection('documentTypesReceipt', 'dropdownDocumentTypes', 'documentType', 'abbreviation', 'abbreviation', value.id)
    }

    public updateFieldsAfterDocumentTypeSelection(value: DocumentTypeAutoCompleteVM): void {
        this.documentTypeHttpService.getSingle(value.id).subscribe({
            next: (response) => {
                const x: DocumentTypeReadDto = response.body
                this.getLastDocumentTypeNo(value.id).subscribe(response => {
                    this.form.patchValue({
                        documentTypeDescription: x.description,
                        invoiceNo: response.body + 1,
                        batch: x.batch
                    })
                })
            },
            error: (errorFromInterceptor) => {
                this.dialogService.open(this.messageDialogService.filterResponse(errorFromInterceptor), 'error', ['ok'])
            }
        })

    }

    public openOrCloseAutoComplete(trigger: MatAutocompleteTrigger, element: any): void {
        this.helperService.openOrCloseAutocomplete(this.form, element, trigger)
    }

    //#endregion

    //#region private methods

    private filterAutocomplete(array: string, field: string, value: any): any[] {
        if (typeof value !== 'object') {
            const filtervalue = value.toLowerCase()
            return this[array].filter((element: { [x: string]: string; }) =>
                element[field].toLowerCase().startsWith(filtervalue))
        }
    }

    private flattenForm(): ReceiptWriteDto {
        return this.receiptHelperService.flattenForm(this.form.value)
    }

    private getLastDocumentTypeNo(id: number): Observable<any> {
        return this.documentTypeHttpService.getLastDocumentTypeNo(id)
    }

    private getRecord(): Promise<any> {
        if (this.recordId) {
            return new Promise((resolve) => {
                const formResolved: FormResolved = this.activatedRoute.snapshot.data['receiptForm']
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
            invoiceId: '',
            date: [new Date(), [Validators.required]],
            tripDate: [new Date(), [Validators.required]],
            customer: ['', [Validators.required, ValidationService.RequireAutocomplete]],
            documentType: ['', [Validators.required, ValidationService.RequireAutocomplete]],
            paymentMethod: ['', [Validators.required, ValidationService.RequireAutocomplete]],
            shipOwner: ['', [Validators.required, ValidationService.RequireAutocomplete]],
            documentTypeDescription: '',
            batch: '',
            invoiceNo: 0,
            grossAmount: [0, [Validators.required, Validators.min(1), Validators.max(99999)]],
            remarks: ['', Validators.maxLength(128)],
            isEmailSent: false,
            isCancelled: false,
            postAt: [''],
            postUser: [''],
            putAt: [''],
            putUser: [''],
        })
    }

    private populateDocumentTypes(): void {
        if (this.recordId) {
            this.populateDocumentTypesAfterShipOwnerSelection('documentTypesReceipt', 'dropdownDocumentTypes', 'documentType', 'abbreviation', 'abbreviation', this.form.value.shipOwner.id)
        }
    }

    private populateDocumentTypesAfterShipOwnerSelection(dexieTable: string, filteredTable: string, formField: string, modelProperty: string, orderBy: string, shipOwnerId: number): void {
        this.dexieService.table(dexieTable).orderBy(orderBy).toArray().then((response) => {
            this[dexieTable] = response.filter(x => x.shipOwner.id == shipOwnerId).filter(x => x.isActive)
            this[filteredTable] = this.form.get(formField).valueChanges.pipe(startWith(''), map(value => this.filterAutocomplete(dexieTable, modelProperty, value)))
        })
    }

    private populateDropdowns(): void {
        this.populateDropdownFromDexieDB('customers', 'dropdownCustomers', 'customer', 'description', 'description')
        this.populateDropdownFromDexieDB('paymentMethods', 'dropdownPaymentMethods', 'paymentMethod', 'description', 'description')
        this.populateDropdownFromDexieDB('shipOwners', 'dropdownShipOwners', 'shipOwner', 'description', 'description')
    }

    private populateDropdownFromDexieDB(dexieTable: string, filteredTable: string, formField: string, modelProperty: string, orderBy: string): void {
        this.dexieService.table(dexieTable).orderBy(orderBy).toArray().then((response) => {
            this[dexieTable] = this.recordId == undefined ? response.filter(x => x.isActive) : response
            this[filteredTable] = this.form.get(formField).valueChanges.pipe(startWith(''), map(value => this.filterAutocomplete(dexieTable, modelProperty, value)))
        })
    }

    private populateFields(): void {
        if (this.record != undefined) {
            this.form.patchValue({
                invoiceId: this.record.invoiceId,
                date: this.record.date,
                tripDate: this.record.tripDate,
                shipOwner: { 'id': this.record.shipOwner.id, 'description': this.record.shipOwner.description },
                customer: { 'id': this.record.customer.id, 'description': this.record.customer.description },
                documentType: { 'id': this.record.documentType.id, 'abbreviation': this.record.documentType.abbreviation },
                documentTypeDescription: this.record.documentType.description,
                invoiceNo: this.record.invoiceNo,
                batch: this.record.documentType.batch,
                paymentMethod: { 'id': this.record.paymentMethod.id, 'description': this.record.paymentMethod.description },
                grossAmount: this.record.grossAmount,
                remarks: this.record.remarks,
                isEmailSent: this.record.isEmailSent,
                isCancelled: this.record.isCancelled,
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

    private saveRecord(receipt: ReceiptWriteDto): void {
        this.receiptHttpService.save(receipt).subscribe({
            next: () => {
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

    get date(): AbstractControl {
        return this.form.get('date')
    }

    get tripDate(): AbstractControl {
        return this.form.get('tripDate')
    }

    get shipOwner(): AbstractControl {
        return this.form.get('shipOwner')
    }

    get documentType(): AbstractControl {
        return this.form.get('documentType')
    }

    get paymentMethod(): AbstractControl {
        return this.form.get('paymentMethod')
    }

    get customer(): AbstractControl {
        return this.form.get('customer')
    }

    get grossAmount(): AbstractControl {
        return this.form.get('grossAmount')
    }

    get remarks(): AbstractControl {
        return this.form.get('remarks')
    }

    //#endregion

}
