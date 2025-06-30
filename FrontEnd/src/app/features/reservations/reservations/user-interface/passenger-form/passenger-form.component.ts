import { Observable } from 'rxjs'
import { Component, Inject, NgZone } from '@angular/core'
import { DateAdapter } from '@angular/material/core'
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms'
import { MatAutocompleteTrigger } from '@angular/material/autocomplete'
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog'
import { map, startWith } from 'rxjs/operators'
// Custom
import { CryptoService } from 'src/app/shared/services/crypto.service'
import { DateHelperService } from 'src/app/shared/services/date-helper.service'
import { DexieService } from 'src/app/shared/services/dexie.service'
import { HelperService } from 'src/app/shared/services/helper.service'
import { InputTabStopDirective } from 'src/app/shared/directives/input-tabstop.directive'
import { LocalStorageService } from 'src/app/shared/services/local-storage.service'
import { MessageInputHintService } from 'src/app/shared/services/message-input-hint.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { NationalityVM } from './../../classes/view-models/passenger/nationality-vm'
import { PassengerReadDto } from '../../classes/dtos/form/passenger-read-dto'
import { SessionStorageService } from 'src/app/shared/services/session-storage.service'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'
import { ValidationService } from 'src/app/shared/services/validation.service'

@Component({
    selector: 'passenger-form',
    templateUrl: './passenger-form.component.html',
    styleUrls: ['./passenger-form.component.css']
})

export class PassengerFormComponent {

    //#region common

    private record: PassengerReadDto
    public feature = 'passengerForm'
    public featureIcon = 'passenger'
    public form: FormGroup
    public icon = 'arrow_back'
    public input: InputTabStopDirective
    public parentUrl = null

    //#endregion

    //#region autocompletes

    public isAutoCompleteDisabled = true
    public dropdownGenders: Observable<SimpleEntity[]>
    public dropdownNationalities: Observable<NationalityVM[]>

    //#endregion

    constructor(@Inject(MAT_DIALOG_DATA) public data: PassengerReadDto, private cryptoService: CryptoService, private dateAdapter: DateAdapter<any>, private dateHelperService: DateHelperService, private dexieService: DexieService, private dialogRef: MatDialogRef<PassengerFormComponent>, private formBuilder: FormBuilder, private helperService: HelperService, private localStorageService: LocalStorageService, private messageHintService: MessageInputHintService, private messageLabelService: MessageLabelService, private ngZone: NgZone, private sessionStorageService: SessionStorageService) {
        this.record = data
    }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
        this.populateDropdowns()
        this.populateFields()
        this.getNationalityFromStorage()
        this.setLocale()
    }

    //#endregion

    //#region public methods

    public autocompleteFields(fieldName: any, object: any): any {
        return object ? object[fieldName] : undefined
    }

    public checkForEmptyAutoComplete(event: { target: { value: any } }): void {
        if (event.target.value == '') this.isAutoCompleteDisabled = true
    }

    public convertFutureDateToPast(): void {
        this.form.patchValue({
            birthdate: this.dateHelperService.convertFutureDateToPast(this.form.value.birthdate)
        })
    }

    public convertToUpperCase(fieldName: string): void {
        this.form.patchValue({
            [fieldName]: this.form.controls[fieldName].value.toUpperCase()
        })
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

    public isAdmin(): boolean {
        return this.cryptoService.decrypt(this.sessionStorageService.getItem('isAdmin')) == 'true' ? true : false
    }

    public openOrCloseAutoComplete(trigger: MatAutocompleteTrigger, element: any): void {
        this.helperService.openOrCloseAutocomplete(this.form, element, trigger)
    }

    public onClose(): void {
        this.dialogRef.close()
    }

    public onSave(): void {
        this.storeNationality()
        this.closeDialog()
    }

    public updateFieldsAfterNationalitySelection(value: NationalityVM): void {
        this.form.patchValue({
            nationality: {
                'id': value.id,
                'description': value.description,
                'code': value.code,
            }
        })
    }

    //#endregion

    //#region private methods

    private assignTempIdToNewPassenger(): number {
        return Math.round(Math.random() * new Date().getMilliseconds())
    }

    private closeDialog(): void {
        this.ngZone.run(() => {
            this.dialogRef.close(this.flattenForm())
        })
    }

    private filterAutocomplete(array: string, field: string, value: any): any[] {
        if (typeof value !== 'object') {
            const filtervalue = value.toLowerCase()
            return this[array].filter((element: { [x: string]: string }) =>
                element[field].toLowerCase().startsWith(filtervalue))
        }
    }

    private flattenForm(): any {
        return {
            id: this.form.value.id == 0 ? this.assignTempIdToNewPassenger() : this.form.value.id,
            reservationId: this.form.value.reservationId,
            lastname: this.form.value.lastname,
            firstname: this.form.value.firstname,
            occupantId: 2,
            birthdate: this.dateHelperService.formatDateToIso(new Date(this.form.value.birthdate)),
            nationality: this.form.value.nationality,
            gender: this.form.value.gender,
            specialCare: this.form.value.specialCare,
            remarks: this.form.value.remarks,
            isBoarded: this.form.value.isBoarded
        }
    }

    private focusOnField(): void {
        this.helperService.focusOnField()
    }

    private getNationalityFromStorage(): void {
        if (this.form.value.id == 0) {
            try {
                const x = JSON.parse(this.sessionStorageService.getItem('nationality'))
                this.form.patchValue({
                    nationality: {
                        'id': x.id,
                        'description': x.description,
                        'code': x.code
                    }
                })
            } catch {
                // do nothing - nationality is not stored
            }
        }
    }

    private initForm(): void {
        this.form = this.formBuilder.group({
            id: this.data.id,
            reservationId: this.data.reservationId,
            gender: ['', [Validators.required, ValidationService.RequireAutocomplete]],
            nationality: ['', [Validators.required, ValidationService.RequireAutocomplete]],
            lastname: ['', [Validators.required, ValidationService.shouldBeCapitalLetterOrSpace, Validators.maxLength(128)]],
            firstname: ['', [Validators.required, ValidationService.shouldBeCapitalLetterOrSpace, Validators.maxLength(128)]],
            birthdate: ['', [Validators.required, ValidationService.ageIsLessThanOneHundredYears]],
            specialCare: ['', Validators.maxLength(128)],
            remarks: ['', Validators.maxLength(128)],
            isBoarded: [{ value: false, disabled: !this.isAdmin }],
        })
    }

    private populateDropdowns(): void {
        this.populateDropdownFromDexieDB('genders', 'dropdownGenders', 'gender', 'description', 'description')
        this.populateDropdownFromDexieDB('nationalities', 'dropdownNationalities', 'nationality', 'description', 'description')
    }

    private populateDropdownFromDexieDB(dexieTable: string, filteredTable: string, formField: string, modelProperty: string, orderBy: string): void {
        this.dexieService.table(dexieTable).orderBy(orderBy).toArray().then((response) => {
            this[dexieTable] = this.record.reservationId.toString() == '' ? response.filter(x => x.isActive) : response
            this[filteredTable] = this.form.get(formField).valueChanges.pipe(startWith(''), map(value => this.filterAutocomplete(dexieTable, modelProperty, value)))
        })
    }

    private populateFields(): void {
        if (this.record.id != 0) {
            this.form.setValue({
                id: this.record.id,
                reservationId: this.record.reservationId,
                gender: { 'id': this.record.gender.id, 'description': this.record.gender.description },
                nationality: { 'id': this.record.nationality.id, 'code': this.record.nationality.code, 'description': this.record.nationality.description },
                lastname: this.record.lastname,
                firstname: this.record.firstname,
                birthdate: this.record.birthdate,
                specialCare: this.record.specialCare,
                remarks: this.record.remarks,
                isBoarded: this.record.isBoarded
            })
        }
    }

    private setLocale(): void {
        this.dateAdapter.setLocale(this.localStorageService.getLanguage())
    }

    private storeNationality(): void {
        this.sessionStorageService.saveItem('nationality', JSON.stringify(this.form.value.nationality))
    }

    //#endregion

    //#region getters

    get lastname(): AbstractControl {
        return this.form.get('lastname')
    }

    get firstname(): AbstractControl {
        return this.form.get('firstname')
    }

    get nationality(): AbstractControl {
        return this.form.get('nationality')
    }

    get gender(): AbstractControl {
        return this.form.get('gender')
    }

    get birthdate(): AbstractControl {
        return this.form.get('birthdate')
    }

    get specialCare(): AbstractControl {
        return this.form.get('specialCare')
    }

    get remarks(): AbstractControl {
        return this.form.get('remarks')
    }

    get isBoarded(): AbstractControl {
        return this.form.get('isBoarded')
    }

    //#endregion

}
