import { ActivatedRoute, Router } from '@angular/router'
import { Component } from '@angular/core'
import { DateAdapter } from '@angular/material/core'
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms'
import { MatAutocompleteTrigger } from '@angular/material/autocomplete'
import { Observable, map, startWith } from 'rxjs'
// Custom
import { DexieService } from 'src/app/shared/services/dexie.service'
import { DialogService } from 'src/app/shared/services/modal-dialog.service'
import { DocumentTypeHelperService } from '../classes/services/docymentType-helper.service'
import { DocumentTypeHttpService } from '../classes/services/documentType-http.service'
import { DocumentTypeReadDto } from '../classes/dtos/documentType-read-dto'
import { DocumentTypeWriteDto } from '../classes/dtos/documentType-write-dto'
import { EmojiService } from 'src/app/shared/services/emoji.service'
import { FormResolved } from 'src/app/shared/classes/form-resolved'
import { HelperService } from 'src/app/shared/services/helper.service'
import { InputTabStopDirective } from 'src/app/shared/directives/input-tabstop.directive'
import { InteractionService } from 'src/app/shared/services/interaction.service'
import { LocalStorageService } from 'src/app/shared/services/local-storage.service'
import { MessageDialogService } from 'src/app/shared/services/message-dialog.service'
import { MessageInputHintService } from 'src/app/shared/services/message-input-hint.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'
import { ValidationService } from 'src/app/shared/services/validation.service'
import { environment } from 'src/environments/environment'

@Component({
    selector: 'documentType-form',
    templateUrl: './documentType-form.component.html',
    styleUrls: ['../../../../../assets/styles/custom/forms.css', './documentType-form.component.css']
})

export class DocumentTypeFormComponent {

    //#region common

    private record: DocumentTypeReadDto
    private recordId: string
    public feature = 'documentTypeForm'
    public featureIcon = 'documentTypes'
    public form: FormGroup
    public icon = 'arrow_back'
    public input: InputTabStopDirective
    public parentUrl = '/documentTypes'

    //#endregion

    //#region autocompletes

    public isAutoCompleteDisabled = true
    public dropdownShips: Observable<SimpleEntity[]>
    public dropdownShipOwners: Observable<SimpleEntity[]>

    //#endregion


    constructor(private activatedRoute: ActivatedRoute, private documentTypeHelperService: DocumentTypeHelperService, private documentTypeHttpService: DocumentTypeHttpService, private dateAdapter: DateAdapter<any>, private dexieService: DexieService, private dialogService: DialogService, private emojiService: EmojiService, private formBuilder: FormBuilder, private helperService: HelperService, private interactionService: InteractionService, private localStorageService: LocalStorageService, private messageDialogService: MessageDialogService, private messageHintService: MessageInputHintService, private messageLabelService: MessageLabelService, private router: Router) { }

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

    public getEmojiForActiveRecord(isActive: boolean): string {
        return this.emojiService.getEmoji(isActive ? 'active' : 'notActive')
    }

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
                this.documentTypeHttpService.delete(this.form.value.id).subscribe({
                    complete: () => {
                        this.dexieService.remove(this.getDiscriminatorDescription(), this.form.value.id)
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

    private flattenForm(): DocumentTypeWriteDto {
        return {
            id: this.form.value.id != '' ? this.form.value.id : null,
            shipId: this.form.value.ship.id == 0 ? null : this.form.value.ship.id,
            shipOwnerId: this.form.value.shipOwner.id,
            abbreviation: this.form.value.abbreviation,
            abbreviationEn: this.form.value.abbreviationEn,
            description: this.form.value.description,
            batch: this.form.value.batch,
            batchEn: this.form.value.batchEn,
            customers: this.form.value.customers,
            suppliers: this.form.value.suppliers,
            discriminatorId: parseInt(this.form.value.discriminatorId),
            table8_1: this.form.value.table8_1,
            table8_8: this.form.value.table8_8,
            table8_9: this.form.value.table8_9,
            isMyData: this.form.value.isMyData,
            isDefault: this.form.value.isDefault,
            isActive: this.form.value.isActive,
            putAt: this.form.value.putAt
        }
    }

    private focusOnField(): void {
        this.helperService.focusOnField()
    }

    private getDiscriminatorDescription(): string {
        switch (parseInt(this.form.value.discriminatorId)) {
            case 1: return 'documentTypesInvoice'
            case 2: return 'documentTypesReceipt'
        }
    }

    private getRecord(): Promise<any> {
        if (this.recordId != undefined) {
            return new Promise((resolve) => {
                const formResolved: FormResolved = this.activatedRoute.snapshot.data['documentTypeForm']
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
            ship: ['', [Validators.required, ValidationService.RequireAutocomplete]],
            shipOwner: ['', [Validators.required, ValidationService.RequireAutocomplete]],
            abbreviation: ['', [Validators.required, Validators.maxLength(5)]],
            abbreviationEn: ['', [Validators.required, Validators.maxLength(5)]],
            description: ['', [Validators.required, Validators.maxLength(128)]],
            batch: ['', [Validators.required, Validators.maxLength(5)]],
            batchEn: ['', [Validators.required, Validators.maxLength(5)]],
            customers: ['', [Validators.maxLength(1), ValidationService.shouldBeEmptyPlusOrMinus]],
            suppliers: ['', [Validators.maxLength(1), ValidationService.shouldBeEmptyPlusOrMinus]],
            discriminatorId: ['', [Validators.required]],
            isDefault: true,
            isActive: true,
            isMyData: true,
            table8_1: ['', [Validators.maxLength(32)]],
            table8_8: ['', [Validators.maxLength(32)]],
            table8_9: ['', [Validators.maxLength(32)]],
            postAt: [''],
            postUser: [''],
            putAt: [''],
            putUser: ['']
        })
    }

    private populateDropdowns(): void {
        this.populateDropdownFromDexieDB('ships', 'dropdownShips', 'ship', 'description', 'description', true)
        this.populateDropdownFromDexieDB('shipOwners', 'dropdownShipOwners', 'shipOwner', 'description', 'description', false)
    }

    private populateDropdownFromDexieDB(dexieTable: string, filteredTable: string, formField: string, modelProperty: string, orderBy: string, includeWildCard: boolean): void {
        this.dexieService.table(dexieTable).orderBy(orderBy).toArray().then((response) => {
            this[dexieTable] = this.recordId == undefined ? response.filter(x => x.isActive) : response
            includeWildCard ? this[dexieTable].unshift({ 'id': '0', 'description': '[' + this.emojiService.getEmoji('wildcard') + ']', 'isActive': true }) : null
            this[filteredTable] = this.form.get(formField).valueChanges.pipe(startWith(''), map(value => this.filterAutocomplete(dexieTable, modelProperty, value)))
        })
    }

    private populateFields(): void {
        if (this.record != undefined) {
            this.form.setValue({
                id: this.record.id,
                ship: { 'id': this.record.ship.id, 'description': this.record.ship.id == 0 ? this.emojiService.getEmoji('wildcard') : this.record.ship.description },
                shipOwner: { 'id': this.record.shipOwner.id, 'description': this.record.shipOwner.description },
                abbreviation: this.record.abbreviation,
                abbreviationEn: this.record.abbreviationEn,
                description: this.record.description,
                batch: this.record.batch,
                batchEn: this.record.batchEn,
                customers: this.record.customers,
                suppliers: this.record.suppliers,
                discriminatorId: this.record.discriminatorId.toString(),
                isDefault: this.record.isDefault,
                isActive: this.record.isActive,
                isMyData: this.record.isMyData,
                table8_1: this.record.table8_1,
                table8_8: this.record.table8_8,
                table8_9: this.record.table8_9,
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

    private saveRecord(documentType: DocumentTypeWriteDto): void {
        this.documentTypeHttpService.save(documentType).subscribe({
            next: (response) => {
                this.documentTypeHelperService.updateBrowserStorageAfterApiUpdate(this.getDiscriminatorDescription(), response.body)
                this.helperService.doPostSaveFormTasks(this.messageDialogService.success(), 'ok', this.parentUrl, true)
            },
            error: (errorFromInterceptor) => {
                this.dialogService.open(this.messageDialogService.filterResponse(errorFromInterceptor), 'error', ['ok'])
            }
        })
    }

    private setLocale(): void {
        this.dateAdapter.setLocale(this.localStorageService.getLanguage())
    }

    private subscribeToInteractionService(): void {
        this.interactionService.refreshDateAdapter.subscribe(() => {
            this.setLocale()
        })
    }

    private setRecordId(): void {
        this.activatedRoute.params.subscribe(x => {
            this.recordId = x.id
        })
    }

    //#endregion

    //#region getters

    get ship(): AbstractControl {
        return this.form.get('ship')
    }

    get shipOwner(): AbstractControl {
        return this.form.get('shipOwner')
    }

    get abbreviation(): AbstractControl {
        return this.form.get('abbreviation')
    }

    get abbreviationEn(): AbstractControl {
        return this.form.get('abbreviationEn')
    }

    get description(): AbstractControl {
        return this.form.get('description')
    }

    get batch(): AbstractControl {
        return this.form.get('batch')
    }

    get batchEn(): AbstractControl {
        return this.form.get('batchEn')
    }

    get customers(): AbstractControl {
        return this.form.get('customers')
    }

    get suppliers(): AbstractControl {
        return this.form.get('suppliers')
    }

    get table8_1(): AbstractControl {
        return this.form.get('table8_1')
    }

    get table8_8(): AbstractControl {
        return this.form.get('table8_8')
    }

    get table8_9(): AbstractControl {
        return this.form.get('table8_9')
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
