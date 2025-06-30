import { Component } from '@angular/core'
import { DateAdapter } from '@angular/material/core'
import { FormGroup, FormBuilder, Validators, FormArray, FormControl } from '@angular/forms'
import { Router } from '@angular/router'
// Custom
import { CryptoService } from 'src/app/shared/services/crypto.service'
import { DateHelperService } from 'src/app/shared/services/date-helper.service'
import { DexieService } from 'src/app/shared/services/dexie.service'
import { HelperService } from 'src/app/shared/services/helper.service'
import { InteractionService } from 'src/app/shared/services/interaction.service'
import { LedgerCriteriaVM } from '../../classes/view-models/criteria/ledger-criteria-vm'
import { LocalStorageService } from 'src/app/shared/services/local-storage.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { SessionStorageService } from 'src/app/shared/services/session-storage.service'
import { SimpleEntity } from './../../../../../shared/classes/simple-entity'
import { SimpleCriteriaEntity } from 'src/app/shared/classes/simple-criteria-entity'

@Component({
    selector: 'ledger-criteria',
    templateUrl: './ledger-criteria.component.html',
    styleUrls: ['../../../../../../assets/styles/custom/lists.css', './ledger-criteria.component.css']
})

export class LedgerCriteriaComponent {

    //#region common

    public feature = 'ledgerCriteria'
    public featureIcon = 'ledgers'
    public form: FormGroup
    public icon = 'home'
    public parentUrl = '/home'
    private criteria: LedgerCriteriaVM

    //#endregion

    //#region tables

    public customersCriteria: SimpleCriteriaEntity[]
    public destinationsCriteria: SimpleCriteriaEntity[]
    public portsCriteria: SimpleCriteriaEntity[]
    public shipsCriteria: SimpleCriteriaEntity[]
    public selectedCustomers: SimpleEntity[]
    public selectedDestinations: SimpleEntity[]
    public selectedPorts: SimpleEntity[]
    public selectedShips: SimpleEntity[]

    //#endregion

    constructor(private cryptoService: CryptoService, private dateAdapter: DateAdapter<any>, private dateHelperService: DateHelperService, private dexieService: DexieService, private formBuilder: FormBuilder, private helperService: HelperService, private interactionService: InteractionService, private localStorageService: LocalStorageService, private messageLabelService: MessageLabelService, private router: Router, private sessionStorageService: SessionStorageService) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
        this.populateDropdowns()
        this.populateFieldsFromStoredVariables()
        this.getConnectedUserRole()
        this.populateCheckboxesFromForm()
        this.setLocale()
        this.subscribeToInteractionService()
        this.setTabTitle()
        this.setSidebarsHeight()
    }

    //#endregion

    //#region public methods

    public doTasks(): void {
        this.storeCriteria()
        this.navigateToList()
    }

    public getDateRange(): any[] {
        const x = []
        x.push(this.form.value.fromDate)
        x.push(this.form.value.toDate)
        return x
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public isAdmin(): boolean {
        return this.cryptoService.decrypt(this.sessionStorageService.getItem('isAdmin')) == 'true' ? true : false
    }

    public patchFormWithSelectedArrays(event: SimpleEntity[], name: string): void {
        const x = this.form.controls[name] as FormArray
        x.controls = []
        x.value.pop()
        this.form.patchValue({
            [name]: ['']
        })
        event.forEach(element => {
            x.push(new FormControl({
                'id': element.id,
                'description': element.description
            }))
        })
    }

    public patchFormWithSelectedDateRange(event: any): void {
        this.form.patchValue({
            fromDate: this.dateHelperService.formatDateToIso(new Date(event.value.fromDate)),
            toDate: this.dateHelperService.formatDateToIso(new Date(event.value.toDate))
        })
    }

    public setDatepickerWidth(): any {
        setTimeout(() => {
            const x = document.getElementById('form')
            if (document.contains(x)) {
                return x.lastChild.childNodes.length == 5 ? 25 : 33
            }
        }, 1000)
    }

    //#endregion

    //#region private methods

    private addSelectedCriteriaFromStorage(arrayName: string): void {
        const x = this.form.controls[arrayName] as FormArray
        this.criteria[arrayName].forEach((element: any) => {
            x.push(new FormControl({
                'id': element.id,
                'description': element.description
            }))
        })
    }

    public getConnectedUserRole(): boolean {
        return this.cryptoService.decrypt(this.sessionStorageService.getItem('isAdmin')) == 'true'
    }

    private initForm(): void {
        this.form = this.formBuilder.group({
            fromDate: [new Date(), [Validators.required]],
            toDate: [new Date(), [Validators.required]],
            selectedCustomers: this.formBuilder.array([], this.cryptoService.decrypt(this.sessionStorageService.getItem('isAdmin')) == 'true' ? Validators.required : null),
            selectedDestinations: this.formBuilder.array([], Validators.required),
            selectedPorts: this.formBuilder.array([], Validators.required),
            selectedShips: this.formBuilder.array([], Validators.required)
        })
    }

    private navigateToList(): void {
        this.router.navigate(['reservation-ledgers/list'])
    }

    private populateDropdowns(): void {
        this.populateDropdownFromDexieDB('customersCriteria', 'description')
        this.populateDropdownFromDexieDB('destinationsCriteria', 'description')
        this.populateDropdownFromDexieDB('portsCriteria', 'description')
        this.populateDropdownFromDexieDB('shipsCriteria', 'description')
    }

    private populateDropdownFromDexieDB(dexieTable: string, orderBy: string): void {
        this.dexieService.table(dexieTable).orderBy(orderBy).toArray().then((response) => {
            this[dexieTable] = response
        })
    }

    private populateCheckboxesFromForm(): void {
        this.selectedCustomers = this.form.value.selectedCustomers
        this.selectedDestinations = this.form.value.selectedDestinations
        this.selectedPorts = this.form.value.selectedPorts
        this.selectedShips = this.form.value.selectedShips
    }

    private populateFieldsFromStoredVariables(): void {
        if (this.sessionStorageService.getItem('ledger-criteria')) {
            this.criteria = JSON.parse(this.sessionStorageService.getItem('ledger-criteria'))
            this.form.patchValue({
                fromDate: this.criteria.fromDate,
                selectedCustomers: this.addSelectedCriteriaFromStorage('selectedCustomers'),
                selectedDestinations: this.addSelectedCriteriaFromStorage('selectedDestinations'),
                selectedPorts: this.addSelectedCriteriaFromStorage('selectedPorts'),
                selectedships: this.addSelectedCriteriaFromStorage('selectedShips'),
            })
        }
    }

    private setLocale(): void {
        this.dateAdapter.setLocale(this.localStorageService.getLanguage())
    }

    private setSidebarsHeight(): void {
        this.helperService.setSidebarsTopMargin('0')
    }

    private setTabTitle(): void {
        this.helperService.setTabTitle(this.feature)
    }

    private storeCriteria(): void {
        this.sessionStorageService.saveItem('ledger-criteria', JSON.stringify(this.form.value))
    }

    private subscribeToInteractionService(): void {
        this.interactionService.refreshDateAdapter.subscribe(() => {
            this.setLocale()
        })
        this.interactionService.refreshTabTitle.subscribe(() => {
            this.setTabTitle()
        })
    }

    //#endregion

}
