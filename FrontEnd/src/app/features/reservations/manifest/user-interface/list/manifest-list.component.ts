import { ActivatedRoute, Router } from '@angular/router'
import { Component, ViewChild } from '@angular/core'
import { Table } from 'primeng/table'
// Custom
import { DateHelperService } from 'src/app/shared/services/date-helper.service'
import { DialogService } from 'src/app/shared/services/modal-dialog.service'
import { EmojiService } from 'src/app/shared/services/emoji.service'
import { HelperService } from 'src/app/shared/services/helper.service'
import { InteractionService } from 'src/app/shared/services/interaction.service'
import { ListResolved } from 'src/app/shared/classes/list-resolved'
import { ManifestCrewVM } from '../../classes/view-models/list/manifest-crew-vm'
import { ManifestCriteriaPanelVM } from '../../classes/view-models/criteria/manifest-criteria-panel-vm'
import { ManifestExportCrewService } from '../../classes/services/manifest-export-crew.service'
import { ManifestExportPassengerService } from '../../classes/services/manifest-export-passenger.service'
import { ManifestPassengerVM } from '../../classes/view-models/list/manifest-passenger-vm'
import { MessageDialogService } from '../../../../../shared/services/message-dialog.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { SessionStorageService } from 'src/app/shared/services/session-storage.service'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'
import { environment } from 'src/environments/environment'
import { ManifestHttpService } from '../../classes/services/manifest.http.service'

@Component({
    selector: 'manifest-list',
    templateUrl: './manifest-list.component.html',
    styleUrls: ['../../../../../../assets/styles/custom/lists.css']
})

export class ManifestListComponent {

    //#region common

    @ViewChild('crewTable') crewTable: Table
    @ViewChild('passengersTable') passengersTable: Table

    public feature = 'manifestList'
    public featureIcon = 'manifest'
    public icon = 'arrow_back'
    public parentUrl = '/manifest'
    public passengers: ManifestPassengerVM[] = []
    public crew: ManifestCrewVM[] = []
    public passengersFilteredCount: number
    public crewFilteredCount: number
    public criteriaPanels: ManifestCriteriaPanelVM

    //#endregion

    //#region specific

    public distinctGenders: SimpleEntity[]
    public distinctNationalities: SimpleEntity[]
    public distinctOccupants: SimpleEntity[]

    //#endregion

    constructor(private manifestHttpService: ManifestHttpService, private manifestExportPassengerService: ManifestExportPassengerService, private manifestExportCrewService: ManifestExportCrewService, private activatedRoute: ActivatedRoute, private dateHelperService: DateHelperService, private dialogService: DialogService, private emojiService: EmojiService, private helperService: HelperService, private interactionService: InteractionService, private messageDialogService: MessageDialogService, private messageLabelService: MessageLabelService, private router: Router, private sessionStorageService: SessionStorageService) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.loadRecords().then(() => {
            this.populateDropdownFilters()
            this.enableDisableFilters()
            this.populateCriteriaPanelsFromStorage()
            this.loadCrew()
        })
        this.subscribeToInteractionService()
        this.setTabTitle()

    }

    //#endregion

    //#region public methods

    public doExportTasks(occupant: string): void {
        occupant == 'passengers'
            ? this.manifestExportPassengerService.exportToExcel(this.manifestExportPassengerService.buildPassengers(this.passengers))
            : this.manifestExportCrewService.exportToExcel(this.manifestExportCrewService.buildCrew(this.crew))
    }

    public filterCrew(event: any): void {
        this.crewFilteredCount = event.filteredValue.length
    }

    public filterPassengers(event: any): void {
        this.passengersFilteredCount = event.filteredValue.length
    }

    public formatDateToLocale(): string {
        return this.criteriaPanels != undefined ? this.dateHelperService.formatISODateToLocale(this.criteriaPanels.date, true, true) : ''
    }

    public getEmoji(emoji: string): string {
        return this.emojiService.getEmoji(emoji)
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public getNationalityIcon(nationalityCode: string): any {
        return environment.nationalitiesIconDirectory + nationalityCode.toLowerCase() + '.png'
    }

    public goBack(): void {
        this.router.navigate([this.parentUrl])
    }

    public isFilterDisabled(): boolean {
        return this.passengers.length == 0
    }

    public resetTableFilters(): void {
        this.helperService.clearTableTextFilters(this.passengersTable, ['lastname', 'firstname'])
    }

    //#endregion

    //#region private methods

    private enableDisableFilters(): void {
        this.passengers.length == 0
            ? this.helperService.disableTableFilters()
            : this.helperService.enableTableFilters()
    }

    private loadRecords(): Promise<any> {
        const promise = new Promise((resolve) => {
            const listResolved: ListResolved = this.activatedRoute.snapshot.data[this.feature]
            if (listResolved.error === null) {
                this.passengers = listResolved.list
                this.passengersFilteredCount = this.passengers.length
                resolve(this.passengers)
            } else {
                this.dialogService.open(this.messageDialogService.filterResponse(listResolved.error), 'error', ['ok']).subscribe(() => {
                    this.goBack()
                })
            }
        })
        return promise
    }

    private populateCriteriaPanelsFromStorage(): void {
        if (this.sessionStorageService.getItem('manifest-criteria')) {
            this.criteriaPanels = JSON.parse(this.sessionStorageService.getItem('manifest-criteria'))
        }
    }

    private loadCrew(): void {
        this.manifestHttpService.getCrew(JSON.parse(this.sessionStorageService.getItem('manifest-criteria')).selectedShips[0].id).subscribe(response => {
            response.forEach(record => {
                this.crew.push(record)
            })
        })
    }

    private populateDropdownFilters(): void {
        if (this.passengers.length > 0) {
            this.distinctGenders = this.helperService.getDistinctRecords(this.passengers, 'gender', 'description')
            this.distinctNationalities = this.helperService.getDistinctRecords(this.passengers, 'nationality', 'description')
        }
    }

    private setTabTitle(): void {
        this.helperService.setTabTitle(this.feature)
    }

    private subscribeToInteractionService(): void {
        this.interactionService.refreshTabTitle.subscribe(() => {
            this.setTabTitle()
        })
    }

    //#endregion

}
