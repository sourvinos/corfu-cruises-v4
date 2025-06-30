import { ActivatedRoute, Router } from '@angular/router'
import { Observable } from 'rxjs'
import { Component } from '@angular/core'
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms'
import { map, startWith } from 'rxjs/operators'
// Custom
import { DexieService } from 'src/app/shared/services/dexie.service'
import { DialogService } from 'src/app/shared/services/modal-dialog.service'
import { FormResolved } from 'src/app/shared/classes/form-resolved'
import { HelperService } from 'src/app/shared/services/helper.service'
import { InputTabStopDirective } from 'src/app/shared/directives/input-tabstop.directive'
import { MatAutocompleteTrigger } from '@angular/material/autocomplete'
import { MessageDialogService } from 'src/app/shared/services/message-dialog.service'
import { MessageInputHintService } from 'src/app/shared/services/message-input-hint.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { ShipHttpService } from '../classes/services/ship-http.service'
import { ShipOwnerBrowserStorageVM } from '../../shipOwners/classes/view-models/shipOwner-autocomplete-vm'
import { ShipOwnerHttpService } from '../../shipOwners/classes/services/shipOwner-http.service'
import { ShipReadDto } from '../classes/dtos/ship-read-dto'
import { ShipWriteDto } from '../classes/dtos/ship-write-dto'
import { ValidationService } from 'src/app/shared/services/validation.service'

@Component({
    selector: 'ship-form',
    templateUrl: './ship-form.component.html',
    styleUrls: ['../../../../../assets/styles/custom/forms.css']
})

export class ShipFormComponent {

    //#region

    private record: ShipReadDto
    private recordId: number
    public feature = 'shipForm'
    public featureIcon = 'ships'
    public form: FormGroup
    public icon = 'arrow_back'
    public input: InputTabStopDirective
    public parentUrl = '/ships'

    //#endregion

    //#region

    public isAutoCompleteDisabled = true
    public dropdownShipOwners: Observable<ShipOwnerBrowserStorageVM[]>

    //#endregion

    constructor(
        private activatedRoute: ActivatedRoute,
        private dexieService: DexieService,
        private dialogService: DialogService,
        private formBuilder: FormBuilder,
        private helperService: HelperService,
        private messageDialogService: MessageDialogService,
        private messageHintService: MessageInputHintService,
        private messageLabelService: MessageLabelService,
        private router: Router,
        private shipHttpService: ShipHttpService,
        private shipOwnerHttpService: ShipOwnerHttpService
    ) { }

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

    public getHint(id: string, minmax = 0): string {
        return this.messageHintService.getDescription(id, minmax)
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public onDelete(): void {
        this.dialogService.open(this.messageDialogService.confirmDelete(), 'question', ['abort', 'ok']).subscribe(response => {
            if (response) {
                this.shipHttpService.delete(this.form.value.id).subscribe({
                    complete: () => {
                        this.dexieService.remove('ships', this.form.value.id)
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
            return this[array].filter((element: { [x: string]: string; }) =>
                element[field].toLowerCase().startsWith(filtervalue))
        }
    }

    private flattenForm(): ShipWriteDto {
        return {
            id: this.form.value.id,
            description: this.form.value.description,
            abbreviation: this.form.value.abbreviation,
            shipOwnerId: this.form.value.shipOwner.id,
            registryNo: this.form.value.registryNo,
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
                const formResolved: FormResolved = this.activatedRoute.snapshot.data['shipForm']
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
            description: ['', [Validators.required, Validators.maxLength(128)]],
            abbreviation: ['', [Validators.required, Validators.maxLength(5)]],
            shipOwner: ['', [Validators.required, ValidationService.RequireAutocomplete]],
            registryNo: ['', [Validators.maxLength(128)]],
            isActive: true,
            postAt: [''],
            postUser: [''],
            putAt: [''],
            putUser: ['']
        })
    }

    private populateDropdowns(): void {
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
            this.form.setValue({
                id: this.record.id,
                description: this.record.description,
                abbreviation: this.record.abbreviation,
                shipOwner: { 'id': this.record.shipOwner.id, 'description': this.record.shipOwner.description },
                registryNo: this.record.registryNo,
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

    private saveRecord(ship: ShipWriteDto): void {
        this.shipHttpService.save(ship).subscribe({
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

    get description(): AbstractControl {
        return this.form.get('description')
    }

    get abbreviation(): AbstractControl {
        return this.form.get('abbreviation')
    }

    get shipOwner(): AbstractControl {
        return this.form.get('shipOwner')
    }

    get registryNo(): AbstractControl {
        return this.form.get('registryNo')
    }

    get isActive(): AbstractControl {
        return this.form.get('isActive')
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
