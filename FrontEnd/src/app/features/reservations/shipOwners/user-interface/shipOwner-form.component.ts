import { ActivatedRoute, Router } from '@angular/router'
import { Component } from '@angular/core'
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms'
import { MatAutocompleteTrigger } from '@angular/material/autocomplete'
import { Observable, map, startWith } from 'rxjs'
// Custom
import { DexieService } from 'src/app/shared/services/dexie.service'
import { DialogService } from 'src/app/shared/services/modal-dialog.service'
import { FormResolved } from 'src/app/shared/classes/form-resolved'
import { HelperService } from 'src/app/shared/services/helper.service'
import { InputTabStopDirective } from 'src/app/shared/directives/input-tabstop.directive'
import { MessageDialogService } from 'src/app/shared/services/message-dialog.service'
import { MessageInputHintService } from 'src/app/shared/services/message-input-hint.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { NationalityVM } from '../../reservations/classes/view-models/passenger/nationality-vm'
import { ShipHttpService } from '../../ships/classes/services/ship-http.service'
import { ShipOwnerAadeHttpService } from '../classes/services/shipOwner-aade-http.service'
import { ShipOwnerAadeRequestVM } from '../classes/view-models/shipOwner-aade-vm'
import { ShipOwnerHttpService } from '../classes/services/shipOwner-http.service'
import { ShipOwnerReadDto } from '../classes/dtos/shipOwner-read-dto'
import { ShipOwnerWriteDto } from '../classes/dtos/shipOwner-write-dto'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'
import { ValidationService } from 'src/app/shared/services/validation.service'
import { environment } from 'src/environments/environment'

@Component({
    selector: 'ship-owner-form',
    templateUrl: './shipOwner-form.component.html',
    styleUrls: ['../../../../../assets/styles/custom/forms.css', './shipOwner-form.component.css']
})

export class ShipOwnerFormComponent {

    //#region common

    private record: ShipOwnerReadDto
    private recordId: number
    public feature = 'shipOwnerForm'
    public featureIcon = 'shipOwners'
    public form: FormGroup
    public icon = 'arrow_back'
    public input: InputTabStopDirective
    public parentUrl = '/shipOwners'

    //#endregion

    //#region autocompletes

    public isAutoCompleteDisabled = true
    public dropdownNationalities: Observable<NationalityVM[]>
    public dropdownTaxOffices: Observable<SimpleEntity[]>
    public dropdowns: Observable<SimpleEntity[]>

    //#endregion

    constructor(private activatedRoute: ActivatedRoute, private dexieService: DexieService, private dialogService: DialogService, private formBuilder: FormBuilder, private helperService: HelperService, private messageDialogService: MessageDialogService, private messageHintService: MessageInputHintService, private messageLabelService: MessageLabelService, private router: Router, private shipHttpService: ShipHttpService, private shipOwnerAadeHttpService: ShipOwnerAadeHttpService, private shipOwnerHttpService: ShipOwnerHttpService) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
        this.setRecordId()
        this.getRecord()
        this.populateFields()
        this.populateDropdowns()
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
                this.shipOwnerHttpService.delete(this.form.value.id).subscribe({
                    complete: () => {
                        this.dexieService.remove('shipOwners', this.form.value.id)
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

    public onSearchAadeRegistry(): void {
        this.shipOwnerAadeHttpService.searchRegistry(this.buildShipOwnerAadeRequesrtVM(this.form.value.vatNumber)).subscribe(response => {
            if (this.isShipOwnerFound(response)) {
                this.processAadeResponse(response)
            } else {
                this.dialogService.open(this.messageDialogService.customerAadeDoesNotExist(), 'error', ['ok']).subscribe()
            }
        })
    }

    public openOrCloseAutoComplete(trigger: MatAutocompleteTrigger, element: any): void {
        this.helperService.openOrCloseAutocomplete(this.form, element, trigger)
    }

    //#endregion

    //#region private methods

    private buildShipOwnerAadeRequesrtVM(vatNumber: string): ShipOwnerAadeRequestVM {
        const x: ShipOwnerAadeRequestVM = {
            username: 'KEP997346439',
            password: 'PKE997346439',
            vatNumber: vatNumber
        }
        return x
    }

    private filterAutocomplete(array: string, field: string, value: any): any[] {
        if (typeof value !== 'object') {
            const filtervalue = value.toLowerCase()
            return this[array].filter((element: { [x: string]: string; }) =>
                element[field].toLowerCase().startsWith(filtervalue))
        }
    }

    private flattenForm(): ShipOwnerWriteDto {
        return {
            id: this.form.value.id,
            nationalityId: this.form.value.nationality.id,
            taxOfficeId: this.form.value.taxOffice.id,
            vatPercent: this.form.value.vatPercent,
            vatPercentId: this.form.value.vatPercentId,
            vatExemptionId: this.form.value.vatExemptionId,
            description: this.form.value.description,
            descriptionEn: this.form.value.descriptionEn,
            vatNumber: this.form.value.vatNumber,
            branch: this.form.value.branch,
            profession: this.form.value.profession,
            street: this.form.value.street,
            number: this.form.value.number,
            postalCode: this.form.value.postalCode,
            city: this.form.value.city,
            personInCharge: this.form.value.personInCharge,
            phones: this.form.value.phones,
            email: this.form.value.email,
            isGroupJP: this.form.value.isGroupJP,
            myDataDemoUrl: this.form.value.myDataDemoUrl,
            myDataDemoUsername: this.form.value.myDataDemoUsername,
            myDataDemoSubscriptionKey: this.form.value.myDataDemoSubscriptionKey,
            myDataLiveUrl: this.form.value.myDataLiveUrl,
            myDataLiveUsername: this.form.value.myDataLiveUsername,
            myDataLiveSubscriptionKey: this.form.value.myDataLiveSubscriptionKey,
            myDataIsActive: this.form.value.myDataIsActive,
            myDataIsDemo: this.form.value.myDataIsDemo,
            oxygenDemoUrl: this.form.value.oxygenDemoUrl,
            oxygenDemoApiKey: this.form.value.oxygenDemoApiKey,
            oxygenLiveUrl: this.form.value.oxygenLiveUrl,
            oxygenLiveApiKey: this.form.value.oxygenLiveApiKey,
            oxygenIsActive: this.form.value.oxygenIsActive,
            oxygenIsDemo: this.form.value.oxygenIsDemo,
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
                const formResolved: FormResolved = this.activatedRoute.snapshot.data['shipOwnerForm']
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
            id: 0,
            nationality: ['', [Validators.required, ValidationService.RequireAutocomplete]],
            taxOffice: ['', [Validators.required, ValidationService.RequireAutocomplete]],
            vatPercent: [0, [Validators.required, Validators.min(0), Validators.max(100)]],
            vatPercentId: [0, [Validators.required, Validators.min(1), Validators.max(9)]],
            vatExemptionId: [0, [Validators.required, Validators.min(0), Validators.max(30)]],
            description: ['', [Validators.required, Validators.maxLength(128)]],
            descriptionEn: ['', [Validators.required, Validators.maxLength(128)]],
            vatNumber: ['', [Validators.required, Validators.maxLength(36)]],
            branch: [0, [Validators.required, Validators.min(0), Validators.max(10)]],
            profession: ['', [Validators.maxLength(128)]],
            street: ['', [Validators.maxLength(128)]],
            number: ['', [Validators.maxLength(4)]],
            postalCode: ['', [Validators.required, Validators.maxLength(10)]],
            city: ['', [Validators.required, Validators.maxLength(128)]],
            personInCharge: ['', [Validators.maxLength(128)]],
            phones: ['', [Validators.maxLength(128)]],
            email: ['', [Validators.email, Validators.maxLength(128)]],
            isGroupJP: false,
            myDataDemoUrl: ['', [Validators.maxLength(256)]],
            myDataDemoUsername: ['', [Validators.maxLength(256)]],
            myDataDemoSubscriptionKey: ['', [Validators.maxLength(256)]],
            myDataLiveUrl: ['', [Validators.maxLength(256)]],
            myDataLiveUsername: ['', [Validators.maxLength(256)]],
            myDataLiveSubscriptionKey: ['', [Validators.maxLength(256)]],
            myDataIsActive: true,
            myDataIsDemo: true,
            oxygenDemoUrl: ['', [Validators.maxLength(256)]],
            oxygenDemoApiKey: ['', [Validators.maxLength(256)]],
            oxygenLiveUrl: ['', [Validators.maxLength(256)]],
            oxygenLiveApiKey: ['', [Validators.maxLength(256)]],
            oxygenIsActive: true,
            oxygenIsDemo: true,
            isActive: true,
            postAt: [''],
            postUser: [''],
            putAt: [''],
            putUser: ['']
        })
    }

    public isShipOwnerFound(response: any): boolean {
        const document = new DOMParser().parseFromString(response.message, 'text/xml')
        return document.querySelector('afm').innerHTML ? true : false
    }

    private populateDropdowns(): void {
        this.populateDropdownFromDexieDB('nationalities', 'dropdownNationalities', 'nationality', 'description', 'description')
        this.populateDropdownFromDexieDB('taxOffices', 'dropdownTaxOffices', 'taxOffice', 'description', 'description')
    }

    private populateDropdownFromDexieDB(dexieTable: string, filteredTable: string, formField: string, modelProperty: string, orderBy: string): void {
        this.dexieService.table(dexieTable).orderBy(orderBy).toArray().then((response) => {
            this[dexieTable] = this.recordId == undefined ? response.filter(x => x.isActive) : response
            this[filteredTable] = this.form.get(formField).valueChanges.pipe(startWith(''), map(value => this.filterAutocomplete(dexieTable, modelProperty, value)))
        })
    }

    private populateFields(): void {
        if (this.recordId != undefined) {
            this.form.setValue({
                id: this.record.id,
                nationality: { 'id': this.record.nationality.id, 'description': this.record.nationality.description },
                taxOffice: { 'id': this.record.taxOffice.id, 'description': this.record.taxOffice.description },
                vatPercent: this.record.vatPercent,
                vatPercentId: this.record.vatPercentId,
                vatExemptionId: this.record.vatExemptionId,
                description: this.record.description,
                descriptionEn: this.record.descriptionEn,
                vatNumber: this.record.vatNumber,
                branch: this.record.branch,
                profession: this.record.profession,
                street: this.record.street,
                number: this.record.number,
                postalCode: this.record.postalCode,
                city: this.record.city,
                personInCharge: this.record.personInCharge,
                phones: this.record.phones,
                email: this.record.email,
                isGroupJP: this.record.isGroupJP,
                myDataDemoUrl: this.record.myDataDemoUrl,
                myDataDemoUsername: this.record.myDataDemoUsername,
                myDataDemoSubscriptionKey: this.record.myDataDemoSubscriptionKey,
                myDataLiveUrl: this.record.myDataLiveUrl,
                myDataLiveUsername: this.record.myDataLiveUsername,
                myDataLiveSubscriptionKey: this.record.myDataLiveSubscriptionKey,
                myDataIsActive: this.record.myDataIsActive,
                myDataIsDemo: this.record.myDataIsDemo,
                oxygenDemoUrl: this.record.oxygenDemoUrl,
                oxygenDemoApiKey: this.record.oxygenDemoApiKey,
                oxygenLiveUrl: this.record.oxygenLiveUrl,
                oxygenLiveApiKey: this.record.oxygenLiveApiKey,
                oxygenIsActive: this.record.oxygenIsActive,
                oxygenIsDemo: this.record.oxygenIsDemo,
                isActive: this.record.isActive,
                postAt: this.record.postAt,
                postUser: this.record.postUser,
                putAt: this.record.putAt,
                putUser: this.record.putUser
            })
        }
    }

    public async processAadeResponse(response: any): Promise<any> {
        const document = new DOMParser().parseFromString(response.message, 'text/xml')
        this.form.patchValue({
            'description': document.querySelector('onomasia').innerHTML,
            'vatNumber': document.querySelector('afm').innerHTML,
            'taxOffice': await this.dexieService.getByDescription('taxOffices', document.querySelector('doy_descr').innerHTML),
            'profession': document.querySelector('firm_act_descr').innerHTML,
            'street': document.querySelector('postal_address').innerHTML,
            'number': document.querySelector('postal_address_no').innerHTML,
            'postalCode': document.querySelector('postal_zip_code').innerHTML,
            'city': document.querySelector('postal_area_description').innerHTML,
            'nationality': await this.dexieService.getByDescription('nationalities', 'GREECE'),
            'vatExemptionId': document.querySelector('normal_vat_system_flag').innerHTML == 'Y' ? 0 : 1
        })
    }

    private resetForm(): void {
        this.form.reset()
    }

    private saveRecord(shipOwner: ShipOwnerWriteDto): void {
        this.shipOwnerHttpService.save(shipOwner).subscribe({
            next: () => {
                this.dexieService.populateTable('shipOwners', this.shipOwnerHttpService)
                this.dexieService.populateTable('ships', this.shipHttpService)
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

    get nationality(): AbstractControl {
        return this.form.get('nationality')
    }

    get taxOffice(): AbstractControl {
        return this.form.get('taxOffice')
    }

    get vatPercent(): AbstractControl {
        return this.form.get('vatPercent')
    }

    get vatPercentId(): AbstractControl {
        return this.form.get('vatPercentId')
    }

    get vatExemptionId(): AbstractControl {
        return this.form.get('vatExemptionId')
    }

    get description(): AbstractControl {
        return this.form.get('description')
    }

    get descriptionEn(): AbstractControl {
        return this.form.get('descriptionEn')
    }

    get vatNumber(): AbstractControl {
        return this.form.get('vatNumber')
    }

    get branch(): AbstractControl {
        return this.form.get('branch')
    }

    get profession(): AbstractControl {
        return this.form.get('profession')
    }

    get street(): AbstractControl {
        return this.form.get('street')
    }

    get number(): AbstractControl {
        return this.form.get('number')
    }

    get postalCode(): AbstractControl {
        return this.form.get('postalCode')
    }

    get city(): AbstractControl {
        return this.form.get('city')
    }

    get personInCharge(): AbstractControl {
        return this.form.get('personInCharge')
    }

    get phones(): AbstractControl {
        return this.form.get('phones')
    }

    get email(): AbstractControl {
        return this.form.get('email')
    }

    get myDataDemoUrl(): AbstractControl {
        return this.form.get('myDataDemoUrl')
    }

    get myDataDemoUsername(): AbstractControl {
        return this.form.get('myDataDemoUsername')
    }

    get myDataDemoSubscriptionKey(): AbstractControl {
        return this.form.get('myDataDemoSubscriptionKey')
    }

    get myDataLiveUrl(): AbstractControl {
        return this.form.get('myDataLiveUrl')
    }

    get myDataLiveUsername(): AbstractControl {
        return this.form.get('myDataLiveUsername')
    }

    get myDataLiveSubscriptionKey(): AbstractControl {
        return this.form.get('myDataLiveSubscriptionKey')
    }

    get oxygenDemoUrl(): AbstractControl {
        return this.form.get('oxygenDemoUrl')
    }

    get oxygenDemoApiKey(): AbstractControl {
        return this.form.get('oxygenDemoApiKey')
    }

    get oxygenLiveUrl(): AbstractControl {
        return this.form.get('oxygenLiveUrl')
    }

    get oxygenLiveApiKey(): AbstractControl {
        return this.form.get('oxygenLiveApiKey')
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
