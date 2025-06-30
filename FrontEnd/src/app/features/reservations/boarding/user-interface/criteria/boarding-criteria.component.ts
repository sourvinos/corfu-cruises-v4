import { Component } from '@angular/core'
import { DateAdapter } from '@angular/material/core'
import { FormGroup, FormBuilder, Validators, FormArray, FormControl } from '@angular/forms'
import { Router } from '@angular/router'
// Custom
import { BoardingCriteriaPanelVM } from '../../classes/view-models/criteria/boarding-criteria-panel-vm'
import { DateHelperService } from 'src/app/shared/services/date-helper.service'
import { DebugDialogService } from '../../../availability/classes/services/debug-dialog.service'
import { DexieService } from 'src/app/shared/services/dexie.service'
import { HelperService } from 'src/app/shared/services/helper.service'
import { InteractionService } from 'src/app/shared/services/interaction.service'
import { LocalStorageService } from 'src/app/shared/services/local-storage.service'
import { MessageInputHintService } from 'src/app/shared/services/message-input-hint.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { SessionStorageService } from 'src/app/shared/services/session-storage.service'
import { SimpleCriteriaEntity } from 'src/app/shared/classes/simple-criteria-entity'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

@Component({
    selector: 'boarding-criteria',
    templateUrl: './boarding-criteria.component.html',
    styleUrls: ['../../../../../../assets/styles/custom/lists.css', './boarding-criteria.component.css']
})

export class BoardingCriteriaComponent {

    //#region common

    public feature = 'boardingCriteria'
    public featureIcon = 'boarding'
    public form: FormGroup
    public icon = 'home'
    public parentUrl = '/home'
    private criteria: BoardingCriteriaPanelVM

    //#endregion

    //#region tables

    public destinationsCriteria: SimpleCriteriaEntity[]
    public portsCriteria: SimpleCriteriaEntity[]
    public shipsCriteria: SimpleCriteriaEntity[]
    public selectedDestinations: SimpleEntity[]
    public selectedPorts: SimpleEntity[]
    public selectedShips: SimpleEntity[]

    //#endregion

    constructor(private dateAdapter: DateAdapter<any>, private dateHelperService: DateHelperService, private debugDialogService: DebugDialogService, private dexieService: DexieService, private formBuilder: FormBuilder, private helperService: HelperService, private interactionService: InteractionService, private localStorageService: LocalStorageService, private messageHintService: MessageInputHintService, private messageLabelService: MessageLabelService, private router: Router, private sessionStorageService: SessionStorageService) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
        this.populateDropdowns()
        this.populateFieldsFromStoredVariables()
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

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public onShowFormValue(): void {
        this.debugDialogService.open(this.form.value, '', ['ok'])
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
                'description': element.description,
                'isActive': element.isActive
            }))
        })
    }

    public patchFormWithSelectedDate(event: any): void {
        this.form.patchValue({
            date: event
        })
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

    private initForm(): void {
        this.form = this.formBuilder.group({
            date: [this.dateHelperService.formatDateToIso(new Date()), Validators.required],
            selectedDestinations: this.formBuilder.array([], Validators.required),
            selectedPorts: this.formBuilder.array([], Validators.required),
            selectedShips: this.formBuilder.array([], Validators.required),
        })
    }

    private navigateToList(): void {
        this.router.navigate(['boarding/list'])
    }

    private populateDropdowns(): void {
        this.populateDropdownFromDexieDB('destinationsCriteria', 'description')
        this.populateDropdownFromDexieDB('portsCriteria', 'description')
        this.populateDropdownFromDexieDB('shipsCriteria', 'description')
    }

    private populateDropdownFromDexieDB(dexieTable: string, orderBy: string): void {
        this.dexieService.table(dexieTable).orderBy(orderBy).toArray().then((response) => {
            this[dexieTable] = response
        })
    }

    private populateFieldsFromStoredVariables(): void {
        if (this.sessionStorageService.getItem('boarding-criteria')) {
            this.criteria = JSON.parse(this.sessionStorageService.getItem('boarding-criteria'))
            this.form.patchValue({
                date: this.criteria.date,
                selectedDestinations: this.addSelectedCriteriaFromStorage('selectedDestinations'),
                selectedports: this.addSelectedCriteriaFromStorage('selectedPorts'),
                selectedships: this.addSelectedCriteriaFromStorage('selectedShips'),
            })
        }
    }

    private populateCheckboxesFromForm(): void {
        this.selectedDestinations = this.form.value.selectedDestinations
        this.selectedPorts = this.form.value.selectedPorts
        this.selectedShips = this.form.value.selectedShips
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
        this.sessionStorageService.saveItem('boarding-criteria', JSON.stringify(this.form.value))
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
