import { ActivatedRoute, Router } from '@angular/router'
import { Component, ViewChild } from '@angular/core'
import { DateAdapter } from '@angular/material/core'
import { MatDialog } from '@angular/material/dialog'
import { Table } from 'primeng/table'
// Custom
import { BoardingCriteriaPanelVM } from '../../../classes/view-models/criteria/boarding-criteria-panel-vm'
import { BoardingDestinationVM } from '../../../classes/view-models/list/boarding-destination-vm'
import { BoardingGroupVM } from '../../../classes/view-models/list/boarding-group-vm'
import { BoardingPDFService } from '../../../classes/services/boarding-pdf.service'
import { BoardingPassengerListComponent } from '../passengers/boarding-passengers.component'
import { BoardingPortVM } from '../../../classes/view-models/list/boarding-port-vm'
import { BoardingReservationVM } from '../../../classes/view-models/list/boarding-reservation-vm'
import { BoardingShipVM } from '../../../classes/view-models/list/boarding-ship-vm'
import { DateHelperService } from 'src/app/shared/services/date-helper.service'
import { DialogService } from 'src/app/shared/services/modal-dialog.service'
import { EmojiService } from 'src/app/shared/services/emoji.service'
import { HelperService } from 'src/app/shared/services/helper.service'
import { InteractionService } from 'src/app/shared/services/interaction.service'
import { ListResolved } from 'src/app/shared/classes/list-resolved'
import { LocalStorageService } from 'src/app/shared/services/local-storage.service'
import { MessageDialogService } from '../../../../../../shared/services/message-dialog.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { SessionStorageService } from 'src/app/shared/services/session-storage.service'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

@Component({
    selector: 'boarding-reservations',
    templateUrl: './boarding-reservations.component.html',
    styleUrls: ['../../../../../../../assets/styles/custom/lists.css', './boarding-reservations.component.css']
})

export class BoardingReservationsComponent {

    //#region common

    @ViewChild('table') table: Table

    private virtualElement: any
    public feature = 'boardingList'
    public featureIcon = 'boarding'
    public icon = 'arrow_back'
    public parentUrl = '/boarding'
    public records: BoardingGroupVM
    public criteriaPanels: BoardingCriteriaPanelVM

    //#endregion

    //#region specific

    public totals = [0, 0, 0]
    public totalsFiltered = [0, 0, 0]
    public remarksRowVisibility: boolean

    public distinctCustomers: SimpleEntity[] = []
    public distinctDestinations: BoardingDestinationVM[] = []
    public distinctDrivers: SimpleEntity[] = []
    public distinctPickupPoints: SimpleEntity[] = []
    public distinctPorts: BoardingPortVM[] = []
    public distinctShips: BoardingShipVM[] = []
    public distinctBoardingStatuses: string[]

    //#endregion

    constructor(private activatedRoute: ActivatedRoute, private dateAdapter: DateAdapter<any>, private dateHelperService: DateHelperService, private dialogService: DialogService, private boardingPDFService: BoardingPDFService, private emojiService: EmojiService, private helperService: HelperService, private interactionService: InteractionService, private localStorageService: LocalStorageService, private messageDialogService: MessageDialogService, private messageLabelService: MessageLabelService, private router: Router, private sessionStorageService: SessionStorageService, public dialog: MatDialog) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.loadRecords()
        this.populateDropdownFilters()
        this.filterTableFromStoredFilters()
        this.updateTotals('totals', this.records.reservations)
        this.updateTotals('totalsFiltered', this.records.reservations)
        this.populateCriteriaPanelsFromStorage()
        this.enableDisableFilters()
        this.getLocale()
        this.subscribeToInteractionService()
        this.setTabTitle()
        this.doVirtualTableTasks()
        this.updateVariables()
        this.updateTableWrapperHeight()
    }

    //#endregion

    //#region public methods

    public createPdf(): void {
        this.boardingPDFService.createPDF(this.records.reservations)
    }

    public filterRecords(event: any): void {
        this.sessionStorageService.saveItem(this.feature + '-' + 'filters', JSON.stringify(this.table.filters))
        this.updateTotals('totalsFiltered', event.filteredValue)
    }

    public formatDateToLocale(date: string, showWeekday = false, showYear = false): string {
        return this.dateHelperService.formatISODateToLocale(date, showWeekday, showYear)
    }

    public getBoardingStatusDescription(reservationStatus: SimpleEntity): string {
        switch (reservationStatus.id) {
            case 1:
                return this.getLabel('boardedFilter')
            case 2:
                return this.getLabel('pendingFilter')
            case 3:
                return this.getLabel('indeterminateFilter')
        }
    }

    public getBoardingStatusIcon(reservationStatus: SimpleEntity): string {
        switch (reservationStatus.id) {
            case 1:
                return this.getEmoji('green-box')
            case 2:
                return this.getEmoji('red-box')
            default:
                return this.getEmoji('yellow-box')
        }
    }

    public getEmoji(emoji: string): string {
        return this.emojiService.getEmoji(emoji)
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public getRemarksRowVisibility(): boolean {
        return this.remarksRowVisibility
    }

    public goBack(): void {
        this.router.navigate([this.parentUrl])
    }

    public hasRemarks(remarks: string): boolean {
        return remarks.length > 0 ? true : false
    }

    public highlightRow(id: any): void {
        this.helperService.highlightRow(id)
    }

    public resetTableFilters(): void {
        this.helperService.clearTableTextFilters(this.table, ['refNo', 'ticketNo', 'totalPax'])
    }

    public showPassengers(reservation: BoardingReservationVM): void {
        this.storeScrollTop()
        this.storeSelectedId(reservation.refNo)
        this.hightlightSavedRow()
        this.showPassengersDialog(reservation)
    }

    public showRemarks(remarks: string): void {
        this.dialogService.open(remarks, 'info', ['ok'])
    }

    public toggleRemarksRowVisibility(): void {
        this.sessionStorageService.saveItem('remarksRowVisibility', this.remarksRowVisibility ? '1' : '0')
    }

    //#endregion

    //#region private methods

    private doVirtualTableTasks(): void {
        setTimeout(() => {
            this.getVirtualElement()
            this.scrollToSavedPosition()
            this.hightlightSavedRow()
        }, 1000)
    }

    private enableDisableFilters(): void {
        this.records.reservations.length == 0 ? this.helperService.disableTableFilters() : this.helperService.enableTableFilters()
    }

    private filterColumn(element: { value: any }, field: string, matchMode: string): void {
        if (element != undefined && (element.value != null || element.value != undefined)) {
            this.table.filter(element.value, field, matchMode)
        }
    }

    private filterTableFromStoredFilters(): void {
        const filters = this.sessionStorageService.getFilters(this.feature + '-' + 'filters')
        if (filters != undefined) {
            setTimeout(() => {
                this.filterColumn(filters.boardingStatusDescription, 'boardingStatusDescription', 'in')
                this.filterColumn(filters.refNo, 'refNo', 'contains')
                this.filterColumn(filters.ticketNo, 'ticketNo', 'contains')
                this.filterColumn(filters.destinationDescription, 'destinationDescription', 'in')
                this.filterColumn(filters.customerDescription, 'customerDescription', 'in')
                this.filterColumn(filters.pickupPointDescription, 'pickupPointDescription', 'in')
                this.filterColumn(filters.driverDescription, 'driverDescription', 'in')
                this.filterColumn(filters.portDescription, 'portDescription', 'in')
                this.filterColumn(filters.shipDescription, 'shipDescription', 'in')
                this.filterColumn(filters.boardingStatus, 'boardingStatus', 'in')
                this.filterColumn(filters.totalPersons, 'totalPersons', 'contains')
            }, 500)
        }
    }

    private getLocale(): void {
        this.dateAdapter.setLocale(this.localStorageService.getLanguage())
    }

    private getVirtualElement(): void {
        this.virtualElement = document.getElementsByClassName('p-scroller-inline')[0]
    }

    private hightlightSavedRow(): void {
        this.helperService.highlightSavedRow(this.feature)
    }

    private loadRecords(): Promise<any> {
        const promise = new Promise((resolve) => {
            const listResolved: ListResolved = this.activatedRoute.snapshot.data[this.feature]
            if (listResolved.error === null) {
                this.records = listResolved.list
                resolve(this.records)
            } else {
                this.dialogService.open(this.messageDialogService.filterResponse(listResolved.error), 'error', ['ok']).subscribe(() => {
                    this.goBack()
                })
            }
        })
        return promise
    }

    private populateCriteriaPanelsFromStorage(): void {
        if (this.sessionStorageService.getItem('boarding-criteria')) {
            this.criteriaPanels = JSON.parse(this.sessionStorageService.getItem('boarding-criteria'))
        }
    }

    private populateDropdownFilters(): void {
        this.distinctCustomers = this.helperService.getDistinctRecords(this.records.reservations, 'customer', 'description')
        this.distinctDestinations = this.helperService.getDistinctRecords(this.records.reservations, 'destination', 'description')
        this.distinctDrivers = this.helperService.getDistinctRecords(this.records.reservations, 'driver', 'description')
        this.distinctPickupPoints = this.helperService.getDistinctRecords(this.records.reservations, 'pickupPoint', 'description')
        this.distinctPorts = this.helperService.getDistinctRecords(this.records.reservations, 'port', 'description')
        this.distinctShips = this.helperService.getDistinctRecords(this.records.reservations, 'ship', 'description')
        this.distinctBoardingStatuses = this.helperService.getDistinctRecords(this.records.reservations, 'boardingStatus', 'description')
    }

    private scrollToSavedPosition(): void {
        this.helperService.scrollToSavedPosition(this.virtualElement, this.feature)
    }

    private setTabTitle(): void {
        this.helperService.setTabTitle(this.feature)
    }

    private showPassengersDialog(reservation: BoardingReservationVM): void {
        const response = this.dialog.open(BoardingPassengerListComponent, {
            data: { reservation: reservation },
            disableClose: true,
            height: '31rem',
            panelClass: 'dialog',
            width: '50rem',
        })
        response.afterClosed().subscribe(result => {
            if (result !== undefined && result == true) {
                this.router.navigateByUrl(this.router.url)
            }
        })
    }

    private storeSelectedId(refNo: string): void {
        this.sessionStorageService.saveItem(this.feature + '-id', refNo)
    }

    private storeScrollTop(): void {
        this.sessionStorageService.saveItem(this.feature + '-scrollTop', this.virtualElement.scrollTop)
    }

    private subscribeToInteractionService(): void {
        this.interactionService.refreshTabTitle.subscribe(() => {
            this.setTabTitle()
        })
    }

    private updateTotals(totalsArray: string, reservations: BoardingReservationVM[]): void {
        const x = [0, 0, 0]
        reservations.forEach(reservation => {
            x[0] += reservation.totalPax
            reservation.passengers.forEach(passenger => {
                passenger.isBoarded ? ++x[1] : x[1]
            })
        })
        x[2] = x[0] - x[1]
        this[totalsArray] = x
    }

    private updateTableWrapperHeight(): void {
        document.getElementById('table-wrapper').style.height = document.getElementById('content').offsetHeight - 100 + 'px'
    }

    private updateVariables(): void {
        this.remarksRowVisibility = this.sessionStorageService.getItem('remarksRowVisibility') != '' ? (this.sessionStorageService.getItem('remarksRowVisibility') == '1' ? true : false) : false
    }

    //#endregion

}
