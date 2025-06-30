import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms'
import { Component, Inject } from '@angular/core'
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog'
import { MatAutocompleteTrigger } from '@angular/material/autocomplete'
import { Observable, map, startWith } from 'rxjs'
// Custom
import { DexieService } from 'src/app/shared/services/dexie.service'
import { DialogService } from 'src/app/shared/services/modal-dialog.service'
import { DocumentTypeAutoCompleteVM } from 'src/app/features/sales/documentTypes/classes/view-models/documentType-autocomplete-vm'
import { DocumentTypeHttpService } from 'src/app/features/sales/documentTypes/classes/services/documentType-http.service'
import { DocumentTypeReadDto } from 'src/app/features/sales/documentTypes/classes/dtos/documentType-read-dto'
import { HelperService } from 'src/app/shared/services/helper.service'
import { InputTabStopDirective } from 'src/app/shared/directives/input-tabstop.directive'
import { InvoiceHelperService } from 'src/app/features/sales/invoices/classes/services/invoice.helper.service'
import { InvoiceHttpDataService } from 'src/app/features/sales/invoices/classes/services/invoice-http-data.service'
import { InvoiceHttpJsonService } from 'src/app/features/sales/invoices/classes/services/invoice-http-json.service'
import { InvoiceHttpXmlService } from 'src/app/features/sales/invoices/classes/services/invoice-http-xml.service'
import { InvoiceJsonHelperService } from 'src/app/features/sales/invoices/classes/services/invoice-json-helper.service'
import { InvoiceWriteDto } from 'src/app/features/sales/invoices/classes/dtos/form/invoice-write-dto'
import { InvoiceXmlHelperService } from 'src/app/features/sales/invoices/classes/services/invoice-xml-helper.service'
import { MessageDialogService } from 'src/app/shared/services/message-dialog.service'
import { MessageInputHintService } from 'src/app/shared/services/message-input-hint.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { PriceHttpService } from 'src/app/features/sales/prices/classes/services/price-http.service'
import { SalesCriteriaVM } from 'src/app/features/sales/invoices/classes/view-models/form/sales-criteria-vm'
import { ShipAutoCompleteVM } from 'src/app/features/reservations/ships/classes/view-models/ship-autocomplete-vm'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'
import { ValidationService } from 'src/app/shared/services/validation.service'

@Component({
    selector: 'invoice-dialog.component',
    templateUrl: './invoice-dialog.component.html',
    styleUrls: ['./invoice-dialog.component.css']
})

export class InvoiceDialogComponent {

    //#region variables

    public feature = 'invoiceForm'
    public featureIcon = 'invoices'
    public icon = 'arrow_back'
    public form: FormGroup
    public input: InputTabStopDirective
    public parentUrl = null
    private isPriceListValid = false

    //#endregion

    //#region autocompletes

    public isAutoCompleteDisabled = true
    public dropdownDocumentTypes: Observable<DocumentTypeAutoCompleteVM[]>
    public dropdownPaymentMethods: Observable<SimpleEntity[]>
    public dropdownShips: Observable<ShipAutoCompleteVM[]>

    //#endregion

    constructor(@Inject(MAT_DIALOG_DATA) public data: any, private dexieService: DexieService, private dialogRef: MatDialogRef<InvoiceDialogComponent>, private dialogService: DialogService, private documentTypeHttpService: DocumentTypeHttpService, private formBuilder: FormBuilder, private helperService: HelperService, private invoiceHelperService: InvoiceHelperService, private invoiceHttpJsonService: InvoiceHttpJsonService, private invoiceHttpService: InvoiceHttpDataService, private invoiceJsonHelperService: InvoiceJsonHelperService, private invoiceXmlHelperService: InvoiceXmlHelperService, private invoiceXmlHttpService: InvoiceHttpXmlService, private messageDialogService: MessageDialogService, private messageHintService: MessageInputHintService, private messageLabelService: MessageLabelService, private priceHttpService: PriceHttpService) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
        this.populateDropdowns()
        this.populateFields()
    }

    async ngAfterViewInit(): Promise<void> {
        await this.getPrices()
        await this.updateShipFromParent()
        await this.updatePaymentMethodWithDefaultValue()
        await this.updateFieldsAfterShipSelection(this.form.value.ship)
        await this.getDefaultDocumentTypeAfterShipSelection()
        await this.updateDocumentTypeFieldsAfterDocumentTypeSelection(this.form.value.documentType)
        await this.onDoCalculations()
    }

    //#endregion

    //#region public methods

    public onSave(): void {
        this.isCustomerDataValid().then((response) => {
            response
                ? this.saveRecord(this.flattenForm())
                : this.dialogService.open(this.messageDialogService.customerDataIsInvalid(), 'error', ['ok'])
        })
    }

    public autocompleteFields(fieldName: any, object: any): any {
        return object ? object[fieldName] : undefined
    }

    public checkForEmptyAutoComplete(event: { target: { value: any } }): void {
        if (event.target.value == '') this.isAutoCompleteDisabled = true
    }

    public enableOrDisableAutoComplete(event: any): void {
        this.isAutoCompleteDisabled = this.helperService.enableOrDisableAutoComplete(event)
    }

    public getPriceListValidity(): boolean {
        return this.isPriceListValid
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public getHint(id: string, minmax = 0): string {
        return this.messageHintService.getDescription(id, minmax)
    }

    public onClose(): void {
        this.dialogRef.close()
    }

    public async onDoCalculations(): Promise<void> {
        this.patchFormWithCalculations(
            this.invoiceHelperService.calculatePortA(this.form.value),
            this.invoiceHelperService.calculatePortB(this.form.value),
            this.invoiceHelperService.calculatePortTotals(this.form.value)
        )
        this.calculateInvoiceSummary()
    }

    public calculateInvoiceSummary(): void {
        const grossAmount = parseFloat(this.form.value.portTotals.total_Amount)
        const vatPercent = parseFloat(this.form.value.vatPercent) / 100
        const netAmount = grossAmount / (1 + vatPercent)
        const vatAmount = netAmount * vatPercent
        this.form.patchValue({
            netAmount: netAmount.toFixed(2),
            vatAmount: vatAmount.toFixed(2),
            grossAmount: grossAmount.toFixed(2)
        })
    }

    public openOrCloseAutoComplete(trigger: MatAutocompleteTrigger, element: any): void {
        this.helperService.openOrCloseAutocomplete(this.form, element, trigger)
    }

    public async updateFieldsAfterShipSelection(value: SimpleEntity): Promise<void> {
        this.populateDocumentTypesAfterShipSelection('documentTypesInvoice', 'dropdownDocumentTypes', 'documentType', 'abbreviation', 'abbreviation', value.id)
    }

    public async updateDocumentTypeFieldsAfterDocumentTypeSelection(value: DocumentTypeAutoCompleteVM): Promise<void> {
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

    //#endregion

    //#region private methods

    private doSubmitTasks(): void {
        this.dexieService.getById('ships', this.form.value.ship.id).then(response => {
            response.shipOwner.isMyData ? this.submitToAade() : this.submitToOxygen()
        })
    }

    private filterAutocomplete(array: string, field: string, value: any): any[] {
        if (typeof value !== 'object') {
            const filtervalue = value.toLowerCase()
            return this[array].filter((element: { [x: string]: string; }) =>
                element[field].toLowerCase().startsWith(filtervalue))
        }
    }

    private flattenForm(): InvoiceWriteDto {
        return this.invoiceHelperService.flattenForm(this.form.value)
    }

    private getLastDocumentTypeNo(id: number): Observable<any> {
        return this.documentTypeHttpService.getLastDocumentTypeNo(id)
    }

    private patchFormWithCalculations(calculationsA: any, calculationsB: any, calculationTotals: any): void {
        this.form.patchValue({
            portA: {
                adults_A_AmountWithTransfer: calculationsA.adults_A_AmountWithTransfer,
                adults_A_AmountWithoutTransfer: calculationsA.adults_A_AmountWithoutTransfer,
                kids_A_AmountWithTransfer: calculationsA.kids_A_AmountWithTransfer,
                kids_A_AmountWithoutTransfer: calculationsA.kids_A_AmountWithoutTransfer,
                total_A_Persons: calculationsA.total_A_Persons,
                total_A_Amount: calculationsA.total_A_Amount
            },
            portB: {
                adults_B_AmountWithTransfer: calculationsB.adults_B_AmountWithTransfer,
                adults_B_AmountWithoutTransfer: calculationsB.adults_B_AmountWithoutTransfer,
                kids_B_AmountWithTransfer: calculationsB.kids_B_AmountWithTransfer,
                kids_B_AmountWithoutTransfer: calculationsB.kids_B_AmountWithoutTransfer,
                total_B_Persons: calculationsB.total_B_Persons,
                total_B_Amount: calculationsB.total_B_Amount
            },
            portTotals: {
                adults_Total_WithTransfer: calculationTotals.adultsWithTransfer,
                adults_TotalAmount_WithTransfer: calculationsA.adults_A_AmountWithTransfer + calculationsB.adults_B_AmountWithTransfer,
                adults_Total_WithoutTransfer: calculationTotals.adultsWithoutTransfer,
                adults_TotalAmount_WithoutTransfer: calculationsA.adults_A_AmountWithoutTransfer + calculationsB.adults_B_AmountWithoutTransfer,
                kids_Total_WithTransfer: calculationTotals.kidsWithTransfer,
                kids_TotalAmount_WithTransfer: calculationsA.kids_A_AmountWithTransfer + calculationsB.kids_B_AmountWithTransfer,
                kids_Total_WithoutTransfer: calculationTotals.kidsWithoutTransfer,
                kids_TotalAmount_WithoutTransfer: calculationsA.kids_A_AmountWithoutTransfer + calculationsB.kids_B_AmountWithoutTransfer,
                free_Total_WithTransfer: calculationTotals.freeWithTransfer,
                free_Total_WithoutTransfer: calculationTotals.freeWithoutTransfer,
                total_Persons: calculationTotals.totalPersons,
                total_Amount: calculationTotals.totalAmount
            }
        })
    }

    private populateDropdowns(): void {
        this.populateDropdownFromDexieDB('paymentMethods', 'dropdownPaymentMethods', 'paymentMethod', 'description', 'description')
        this.populateDropdownFromDexieDB('ships', 'dropdownShips', 'ship', 'description', 'description')
    }

    private populateDocumentTypesAfterShipSelection(dexieTable: string, filteredTable: string, formField: string, modelProperty: string, orderBy: string, shipId: number): void {
        this.dexieService.table(dexieTable).orderBy(orderBy).toArray().then((response) => {
            this[dexieTable] = response.filter(x => x.ship.id == shipId).filter(x => x.isActive)
            this[filteredTable] = this.form.get(formField).valueChanges.pipe(startWith(''), map(value => this.filterAutocomplete(dexieTable, modelProperty, value)))
        })
    }

    private populateDropdownFromDexieDB(dexieTable: string, filteredTable: string, formField: string, modelProperty: string, orderBy: string): void {
        this.dexieService.table(dexieTable).orderBy(orderBy).toArray().then((response) => {
            this[dexieTable] = response.filter(x => x.isActive)
            this[filteredTable] = this.form.get(formField).valueChanges.pipe(startWith(''), map(value => this.filterAutocomplete(dexieTable, modelProperty, value)))
        })
    }

    private initForm(): void {
        this.form = this.formBuilder.group({
            invoiceId: '',
            date: [new Date(), [Validators.required]],
            tripDate: [new Date(), [Validators.required]],
            customer: [''],
            destination: [''],
            documentType: ['', [Validators.required, ValidationService.RequireAutocomplete]],
            documentTypeDescription: '',
            batch: '',
            invoiceNo: 0,
            paymentMethod: ['', [Validators.required, ValidationService.RequireAutocomplete]],
            ship: ['', [Validators.required, ValidationService.RequireAutocomplete]],
            netAmount: [0, ValidationService.isGreaterThanZero],
            vatPercent: [0],
            vatAmount: [0, ValidationService.isGreaterThanZero],
            grossAmount: [0, [Validators.required, Validators.min(1), Validators.max(99999)]],
            portA: this.formBuilder.group({
                id: 0,
                invoiceId: '',
                portId: 1,
                port_A_Description: 'CORFU PORT',
                adults_A_WithTransfer: [this.data[0].adultsWithTransfer, [Validators.required, Validators.min(0), Validators.max(999)]],
                adults_A_PriceWithTransfer: [0, [Validators.required, Validators.min(0), Validators.max(999)]],
                adults_A_AmountWithTransfer: [0, [Validators.required, Validators.min(0), Validators.max(999)]],
                adults_A_WithoutTransfer: [this.data[0].adultsWithoutTransfer, [Validators.required, Validators.min(0), Validators.max(999)]],
                adults_A_PriceWithoutTransfer: [0, [Validators.required, Validators.min(0), Validators.max(999)]],
                adults_A_AmountWithoutTransfer: [0, [Validators.required, Validators.min(0), Validators.max(999)]],
                kids_A_WithTransfer: [this.data[0].kidsWithTransfer, [Validators.required, Validators.min(0), Validators.max(999)]],
                kids_A_PriceWithTransfer: [0, [Validators.required, Validators.min(0), Validators.max(999)]],
                kids_A_AmountWithTransfer: [0, [Validators.required, Validators.min(0), Validators.max(999)]],
                kids_A_WithoutTransfer: [this.data[0].kidsWithoutTransfer, [Validators.required, Validators.min(0), Validators.max(999)]],
                kids_A_PriceWithoutTransfer: [0, [Validators.required, Validators.min(0), Validators.max(999)]],
                kids_A_AmountWithoutTransfer: [0, [Validators.required, Validators.min(0), Validators.max(999)]],
                free_A_WithTransfer: [this.data[0].freeWithTransfer, [Validators.required, Validators.min(0), Validators.max(999)]],
                free_A_WithoutTransfer: [this.data[0].freeWithoutTransfer, [Validators.required, Validators.min(0), Validators.max(999)]],
                total_A_Persons: [this.data[0].total],
                total_A_Amount: [0]
            }),
            portB: this.formBuilder.group({
                id: 0,
                invoiceId: '',
                portId: 2,
                port_B_Description: 'LEFKIMMI PORT',
                adults_B_WithTransfer: [this.data[1].adultsWithTransfer, [Validators.required, Validators.min(0), Validators.max(999)]],
                adults_B_PriceWithTransfer: [0, [Validators.required, Validators.min(0), Validators.max(999)]],
                adults_B_AmountWithTransfer: [0, [Validators.required, Validators.min(0), Validators.max(999)]],
                adults_B_WithoutTransfer: [this.data[1].adultsWithoutTransfer, [Validators.required, Validators.min(0), Validators.max(999)]],
                adults_B_PriceWithoutTransfer: [0, [Validators.required, Validators.min(0), Validators.max(999)]],
                adults_B_AmountWithoutTransfer: [0, [Validators.required, Validators.min(0), Validators.max(999)]],
                kids_B_WithTransfer: [this.data[1].kidsWithTransfer, [Validators.required, Validators.min(0), Validators.max(999)]],
                kids_B_PriceWithTransfer: [0, [Validators.required, Validators.min(0), Validators.max(999)]],
                kids_B_AmountWithTransfer: [0, [Validators.required, Validators.min(0), Validators.max(999)]],
                kids_B_WithoutTransfer: [this.data[1].kidsWithoutTransfer, [Validators.required, Validators.min(0), Validators.max(999)]],
                kids_B_PriceWithoutTransfer: [0, [Validators.required, Validators.min(0), Validators.max(999)]],
                kids_B_AmountWithoutTransfer: [0, [Validators.required, Validators.min(0), Validators.max(999)]],
                free_B_WithTransfer: [this.data[1].freeWithTransfer, [Validators.required, Validators.min(0), Validators.max(999)]],
                free_B_WithoutTransfer: [this.data[1].freeWithoutTransfer, [Validators.required, Validators.min(0), Validators.max(999)]],
                total_B_Persons: [this.data[1].total],
                total_B_Amount: [0]
            }),
            portTotals: this.formBuilder.group({
                port_Totals_Description: '',
                adults_Total_WithTransfer: 0,
                adults_TotalAmount_WithTransfer: 0,
                adults_Total_WithoutTransfer: 0,
                adults_TotalAmount_WithoutTransfer: 0,
                kids_Total_WithTransfer: 0,
                kids_TotalAmount_WithTransfer: 0,
                kids_Total_WithoutTransfer: 0,
                kids_TotalAmount_WithoutTransfer: 0,
                free_Total_WithTransfer: 0,
                free_Total_WithoutTransfer: 0,
                total_Persons: 0,
                total_Amount: 0
            }),
            remarks: ['', Validators.maxLength(128)]
        })
    }

    private isCustomerDataValid(): Promise<any> {
        return new Promise((resolve) => {
            this.invoiceHttpService.validateCustomerData(this.form.value.customer.id).subscribe({
                next: (response) => {
                    resolve(response.body.isValid)
                },
                error: (errorFromInterceptor) => {
                    this.dialogService.open(this.messageDialogService.filterResponse(errorFromInterceptor), 'error', ['ok'])
                }
            })
        })
    }

    private saveRecord(invoice: InvoiceWriteDto): void {
        this.invoiceHttpService.save(invoice).subscribe({
            next: (response) => {
                this.form.patchValue({
                    invoiceId: response.id
                })
                this.doSubmitTasks()
            },
            error: (errorFromInterceptor) => {
                this.dialogService.open(this.messageDialogService.filterResponse(errorFromInterceptor), 'error', ['ok'])
            }
        })
    }

    private submitToAade(): void {
        this.invoiceXmlHttpService.get(this.form.value.invoiceId).subscribe(response => {
            this.invoiceXmlHttpService.uploadInvoice(response.body).subscribe({
                next: (response) => {
                    this.invoiceHttpService.updateInvoiceAade(this.invoiceXmlHelperService.processInvoiceSuccessResponse(response)).subscribe({
                        next: () => {
                            this.helperService.doPostSaveFormTasks(this.messageDialogService.success(), 'ok', this.parentUrl, false).then(() => {
                                this.dialogRef.close()
                            })
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
        })
    }

    private submitToOxygen(): void {
        if (this.form.value.mark == '' || this.form.value.mark == undefined) {
            this.invoiceHttpJsonService.get(this.form.value.invoiceId).subscribe({
                next: (response) => {
                    const x = this.invoiceJsonHelperService.processInvoiceResponse(this.form.value.invoiceId, response)
                    this.invoiceHttpService.updateInvoiceOxygen(x).subscribe({
                        next: () => {
                            if (x.id) {
                                this.helperService.doPostSaveFormTasks(this.messageDialogService.success(), 'ok', this.parentUrl, true).then(() => {
                                    this.dialogRef.close()
                                })
                            } else {
                                this.dialogService.open(this.messageDialogService.providerNotUpdated(), 'error', ['ok']).subscribe(() => {
                                    this.dialogRef.close()
                                })
                            }
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
        } else {
            this.dialogService.open(this.messageDialogService.providerNotUpdated(), 'error', ['ok'])
        }
    }

    private async populateFields(): Promise<void> {
        await this.dexieService.getById('customers', this.data[0].customer.id).then(response => {
            this.form.patchValue({
                customer: {
                    id: response.id,
                    description: response.description
                },
                vatPercent: response.vatPercent
            })
        })
        await this.dexieService.getById('destinations', this.data[0].destination.id).then(response => {
            this.form.patchValue({
                destination: {
                    id: response.id,
                    description: response.description
                }
            })
        })
    }

    private async getPrices(): Promise<void> {
        const x: SalesCriteriaVM = {
            date: this.data[0].date,
            customerId: this.data[0].customer.id,
            destinationId: this.data[0].destination.id,
        }
        if (this.invoiceHelperService.validatePriceRetriever(x)) {
            this.priceHttpService.retrievePrices(x).subscribe({
                next: (response: any) => {
                    if (response.body.length != 2) {
                        this.isPriceListValid = false
                    } else {
                        this.form.patchValue({
                            portA: {
                                adults_A_PriceWithTransfer: response.body[0].adultsWithTransfer,
                                adults_A_PriceWithoutTransfer: response.body[0].adultsWithoutTransfer,
                                kids_A_PriceWithTransfer: response.body[0].kidsWithTransfer,
                                kids_A_PriceWithoutTransfer: response.body[0].kidsWithoutTransfer,
                            },
                            portB: {
                                adults_B_PriceWithTransfer: response.body[1].adultsWithTransfer,
                                adults_B_PriceWithoutTransfer: response.body[1].adultsWithoutTransfer,
                                kids_B_PriceWithTransfer: response.body[1].kidsWithTransfer,
                                kids_B_PriceWithoutTransfer: response.body[1].kidsWithoutTransfer,
                            },
                        })
                        this.isPriceListValid = true
                    }
                },
                error: () => {
                    this.isPriceListValid = false
                }
            })
        } else {
            this.isPriceListValid = false
        }
    }

    private async updateShipFromParent(): Promise<void> {
        this.form.patchValue({
            'ship': await this.dexieService.getByDescription('ships', this.data[0].ship.description),
        })
    }

    private async updatePaymentMethodWithDefaultValue(): Promise<void> {
        this.form.patchValue({
            'paymentMethod': await this.dexieService.getByDefault('paymentMethods', 'isDefault')
        })
    }

    private async getDefaultDocumentTypeAfterShipSelection(): Promise<void> {
        this.form.patchValue({
            'documentType': await this.dexieService.getDefaultDocumentType('documentTypesInvoice', this.form.value.ship.id)
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

    get documentType(): AbstractControl {
        return this.form.get('documentType')
    }

    get paymentMethod(): AbstractControl {
        return this.form.get('paymentMethod')
    }

    get customer(): AbstractControl {
        return this.form.get('customer')
    }

    get destination(): AbstractControl {
        return this.form.get('destination')
    }

    get ship(): AbstractControl {
        return this.form.get('ship')
    }

    get netAmount(): AbstractControl {
        return this.form.get('netAmount')
    }

    get vatPercent(): AbstractControl {
        return this.form.get('vatPercent')
    }

    get vatAmount(): AbstractControl {
        return this.form.get('vatAmount')
    }

    get grossAmount(): AbstractControl {
        return this.form.get('grossAmount')
    }

    get remarks(): AbstractControl {
        return this.form.get('remarks')
    }

    get port_A_Description(): AbstractControl {
        return this.form.get('portA.portDescription')
    }

    get adults_A_WithTransfer(): AbstractControl {
        return this.form.get('portA.adults_A_WithTransfer')
    }

    get adults_A_PriceWithTransfer(): AbstractControl {
        return this.form.get('portA.adults_A_PriceWithTransfer')
    }

    get adults_A_AmountWithTransfer(): AbstractControl {
        return this.form.get('portA.adults_A_AmountWithTransfer')
    }

    get adults_A_WithoutTransfer(): AbstractControl {
        return this.form.get('portA.adults_A_WithoutTransfer')
    }

    get adults_A_PriceWithoutTransfer(): AbstractControl {
        return this.form.get('portA.adults_A_PriceWithoutTransfer')
    }

    get adults_A_AmountWithoutTransfer(): AbstractControl {
        return this.form.get('portA.adults_A_AmountWithoutTransfer')
    }

    get kids_A_WithTransfer(): AbstractControl {
        return this.form.get('portA.kids_A_WithTransfer')
    }

    get kids_A_PriceWithTransfer(): AbstractControl {
        return this.form.get('portA.kids_A_PriceWithTransfer')
    }

    get kids_A_AmountWithTransfer(): AbstractControl {
        return this.form.get('portA.kids_A_AmountWithTransfer')
    }

    get kids_A_WithoutTransfer(): AbstractControl {
        return this.form.get('portA.kids_A_WithoutTransfer')
    }

    get kids_A_PriceWithoutTransfer(): AbstractControl {
        return this.form.get('portA.kids_A_PriceWithoutTransfer')
    }

    get kids_A_AmountWithoutTransfer(): AbstractControl {
        return this.form.get('portA.kids_A_AmountWithoutTransfer')
    }

    get free_A_WithTransfer(): AbstractControl {
        return this.form.get('portA.free_A_WithTransfer')
    }

    get free_A_WithoutTransfer(): AbstractControl {
        return this.form.get('portA.free_A_WithoutTransfer')
    }

    get total_A_Persons(): AbstractControl {
        return this.form.get('total_A_Persons')
    }

    get total_A_Amount(): AbstractControl {
        return this.form.get('total_A_Amount')
    }

    get port_B_Description(): AbstractControl {
        return this.form.get('portB.portDescription')
    }

    get adults_B_WithTransfer(): AbstractControl {
        return this.form.get('portB.adults_B_WithTransfer')
    }

    get adults_B_PriceWithTransfer(): AbstractControl {
        return this.form.get('portB.adults_B_PriceWithTransfer')
    }

    get adults_B_AmountWithTransfer(): AbstractControl {
        return this.form.get('portB.adults_B_AmountWithTransfer')
    }

    get adults_B_WithoutTransfer(): AbstractControl {
        return this.form.get('portB.adults_B_WithoutTransfer')
    }

    get adults_B_PriceWithoutTransfer(): AbstractControl {
        return this.form.get('portB.adults_B_PriceWithoutTransfer')
    }

    get adults_B_AmountWithoutTransfer(): AbstractControl {
        return this.form.get('portB.adults_B_AmountWithoutTransfer')
    }

    get kids_B_WithTransfer(): AbstractControl {
        return this.form.get('portB.kids_B_WithTransfer')
    }

    get kids_B_PriceWithTransfer(): AbstractControl {
        return this.form.get('portB.kids_B_PriceWithTransfer')
    }

    get kids_B_AmountWithTransfer(): AbstractControl {
        return this.form.get('portB.kids_B_AmountWithTransfer')
    }

    get kids_B_WithoutTransfer(): AbstractControl {
        return this.form.get('portB.kids_B_WithoutTransfer')
    }

    get kids_B_PriceWithoutTransfer(): AbstractControl {
        return this.form.get('portB.kids_B_PriceWithoutTransfer')
    }

    get kids_B_AmountWithoutTransfer(): AbstractControl {
        return this.form.get('portB.kids_B_AmountWithoutTransfer')
    }

    get free_B_WithTransfer(): AbstractControl {
        return this.form.get('portB.free_B_WithTransfer')
    }

    get free_B_WithoutTransfer(): AbstractControl {
        return this.form.get('portB.free_B_WithoutTransfer')
    }

    get total_B_Persons(): AbstractControl {
        return this.form.get('total_B_Persons')
    }

    get total_B_Amount(): AbstractControl {
        return this.form.get('total_B_Amount')
    }

    get adults_Total_WithTransfer(): AbstractControl {
        return this.form.get('portTotals.adults_Total_WithTransfer')
    }

    get adults_TotalAmount_WithTransfer(): AbstractControl {
        return this.form.get('portTotals.adults_TotalAmount_WithTransfer')
    }

    get adults_Total_WithoutTransfer(): AbstractControl {
        return this.form.get('portTotals.adults_Total_WithoutTransfer')
    }

    get adults_TotalAmount_WithoutTransfer(): AbstractControl {
        return this.form.get('portTotals.adults_TotalAmount_WithoutTransfer')
    }

    get kids_Total_WithTransfer(): AbstractControl {
        return this.form.get('portTotals.kids_Total_WithTransfer')
    }

    get kids_TotalAmount_WithTransfer(): AbstractControl {
        return this.form.get('portTotals.kids_TotalAmount_WithTransfer')
    }

    get kids_Total_WithoutTransfer(): AbstractControl {
        return this.form.get('portTotals.kids_Total_WithoutTransfer')
    }

    get kids_TotalAmount_WithoutTransfer(): AbstractControl {
        return this.form.get('portTotals.kids_TotalAmount_WithoutTransfer')
    }

    get free_Total_WithTransfer(): AbstractControl {
        return this.form.get('portTotals.free_Total_WithTransfer')
    }

    get free_Total_WithoutTransfer(): AbstractControl {
        return this.form.get('portTotals.free_Total_WithoutTransfer')
    }

    get total_Persons(): AbstractControl {
        return this.form.get('portTotals.total_Persons')
    }

    get total_Amount(): AbstractControl {
        return this.form.get('portTotals.total_Amount')
    }

    //#endregion

}
