import { Component, ViewChild } from '@angular/core'
import { MatDialog } from '@angular/material/dialog'
import { MenuItem } from 'primeng/api'
import { Table } from 'primeng/table'
// Custom
import { CriteriaDateRangeDialogComponent } from '../../../../../shared/components/criteria-date-range-dialog/criteria-date-range-dialog.component'
import { DateHelperService } from '../../../../../shared/services/date-helper.service'
import { DebugDialogService } from '../../../availability/classes/services/debug-dialog.service'
import { DialogService } from '../../../../../shared/services/modal-dialog.service'
import { EmojiService } from '../../../../../shared/services/emoji.service'
import { HelperService } from '../../../../../shared/services/helper.service'
import { InteractionService } from '../../../../../shared/services/interaction.service'
import { MessageDialogService } from '../../../../../shared/services/message-dialog.service'
import { MessageLabelService } from '../../../../../shared/services/message-label.service'
import { ReservationImportDto } from '../../classes/dtos/reservation-import-dto'
import { ReservationImportHttpDataService } from '../../classes/services/reservation-list-http-data.service'
import { ReservationImportListCriteriaVM } from '../../classes/view-models/criteria/reservations-import-list-criteria-vm'
import { ReservationImportListVM } from '../../classes/view-models/list/reservation-import-list-vm'
import { ReservationImportService } from '../../classes/services/reservation-import.service'
import { SessionStorageService } from '../../../../../shared/services/session-storage.service'
import { SimpleEntity } from '../../../../../shared/classes/simple-entity'

@Component({
    selector: 'reservation-import-list',
    templateUrl: './reservation-import-list.component.html',
    styleUrls: ['../../../../../../assets/styles/custom/lists.css', './reservation-import-list.component.css']
})

export class ReservationImportListComponent {

    //#region variables

    @ViewChild('table') table: Table

    private criteria: ReservationImportListCriteriaVM
    private url = 'reservations-import'
    private virtualElement: any
    public feature = 'reservationImportList'
    public featureIcon = 'reservations-import'
    public icon = 'home'
    public parentUrl = '/home'
    public records: ReservationImportListVM[] = []
    public selectedRecords: ReservationImportListVM[] = []
    public recordsFilteredCount = 0
    public recordsFiltered: ReservationImportListVM[]

    //#endregion

    //#region dropdown filters

    public distinctCustomers: SimpleEntity[] = []
    public distinctDestinations: SimpleEntity[] = []
    public distinctPickupPoints: SimpleEntity[] = []
    public distinctStatuses: SimpleEntity[] = []

    //#endregion

    //#region context menu

    public menuItems!: MenuItem[]
    public selectedRecord!: ReservationImportListVM

    //#endregion

    constructor(private dateHelperService: DateHelperService, private debugDialogService: DebugDialogService, private dialogService: DialogService, private emojiService: EmojiService, private helperService: HelperService, private interactionService: InteractionService, private messageDialogService: MessageDialogService, private messageLabelService: MessageLabelService, private reservationImportService: ReservationImportService, private reservationImportHttpService: ReservationImportHttpDataService, private sessionStorageService: SessionStorageService, public dialog: MatDialog) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.setTabTitle()
        this.subscribeToInteractionService()
    }

    //#endregion

    //#region public methods

    public formatDateToLocale(date: string): string {
        return this.dateHelperService.formatISODateToLocale(date)
    }

    public getCriteria(): string {
        return this.criteria
            ? this.dateHelperService.formatISODateToLocale(this.criteria.fromDate) + ' - ' + this.dateHelperService.formatISODateToLocale(this.criteria.toDate)
            : ''
    }

    public getEmoji(anything: any): string {
        return typeof anything == 'string'
            ? this.emojiService.getEmoji(anything)
            : anything ? this.emojiService.getEmoji('green-box') : this.emojiService.getEmoji('red-box')
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public getPickupPointDescription(pickupPoint: SimpleEntity): string {
        return pickupPoint.id != 9999 ? pickupPoint.description : this.emojiService.getEmoji('warning')
    }

    public onClearFilterTasks(): void {
        this.clearFilters()
        this.deleteStoredFilters()
        this.clearSelectedRecords()
        this.initFilteredRecordsCount()
    }

    public onFilter(event: any, column: string, matchMode: string): void {
        if (event) this.table.filter(event, column, matchMode)
    }

    public onHighlightRow(code: any): void {
        this.helperService.highlightRow(code)
    }

    public onImportRecords(): void {
        if (this.isAnyRowSelected()) {
            if (!this.areSelectedRowsValid()) {
                this.dialogService.open(this.messageDialogService.invalidReservationsToImport(), 'error', ['ok'])
            } else {
                this.processReservations(this.reservationImportService.buildReservations(this.selectedRecords))
            }
        } else {
            this.dialogService.open(this.messageDialogService.noRecordsSelected(), 'error', ['ok'])
        }
    }

    public onFilterRecords(event: any): void {
        setTimeout(() => {
            this.sessionStorageService.saveItem(this.feature + '-' + 'filters', JSON.stringify(this.table.filters))
            this.recordsFiltered = event.filteredValue
            this.recordsFilteredCount = event.filteredValue.length
        }, 500)
    }

    public onLoadRecord(code: string): void {
        this.reservationImportHttpService.getByCode(code).subscribe(response => {
            this.debugDialogService.open(response, '', ['ok'])
        })
    }

    public onRefreshList(): void {
        this.buildCriteriaVM(this.criteria).then((response) => {
            this.loadRecords(response).then(() => {
                this.initFilteredRecordsCount()
                this.filterTableFromStoredFilters()
                this.populateDropdownFilters()
                this.clearSelectedRecords()
                this.doVirtualTableTasks()
            })
        })
    }

    public onShowCriteriaDialog(): void {
        const dialogRef = this.dialog.open(CriteriaDateRangeDialogComponent, {
            data: 'reservationImportListCriteria',
            height: '36.0625rem',
            panelClass: 'dialog',
            width: '32rem',
        })
        dialogRef.afterClosed().subscribe(criteria => {
            if (criteria !== undefined) {
                this.onClearFilterTasks()
                this.buildCriteriaVM(criteria).then((response) => {
                    this.loadRecords(response).then(() => {
                        this.initFilteredRecordsCount()
                        this.filterTableFromStoredFilters()
                        this.populateDropdownFilters()
                        this.clearSelectedRecords()
                        this.doVirtualTableTasks()
                    })
                })
            }
        })
    }

    //#endregion

    //#region private methods

    private areSelectedRowsValid(): boolean {
        let x = 0
        this.selectedRecords.forEach(row => {
            if (row.customer.description == '' || row.destination.description == '' || row.pickupPoint.description == '' || row.totalPax == 0) {
                return false
            } else {
                x++
            }
        })
        if (this.selectedRecords.length == x) {
            return true
        }
        return false
    }

    private buildCriteriaVM(event: ReservationImportListCriteriaVM): Promise<any> {
        return new Promise((resolve) => {
            this.criteria = {
                fromDate: event.fromDate,
                toDate: event.toDate
            }
            resolve(this.criteria)
        })
    }

    private clearFilters(): void {
        this.table != undefined
            ? this.helperService.clearTableTextFilters(this.table)
            : null
    }

    private clearSelectedRecords(): void {
        this.selectedRecords = []
    }

    private deleteStoredFilters(): void {
        this.sessionStorageService.deleteItems([{ 'item': 'invoiceList-filters', 'when': 'always' }])
    }

    private doVirtualTableTasks(): void {
        setTimeout(() => {
            this.getVirtualElement()
            this.scrollToSavedPosition()
            this.hightlightSavedRow()
        }, 1000)
    }

    private filterTableFromStoredFilters(): void {
        const filters = this.sessionStorageService.getFilters(this.feature + '-' + 'filters')
        if (filters) {
            setTimeout(() => {
                // this.filterColumn(filters.date, 'date', 'in')
                // this.filterColumn(filters.customer, 'customer', 'in')
                // this.filterColumn(filters.destination, 'destination', 'in')
                // this.filterColumn(filters.ship, 'ship', 'in')
                // this.filterColumn(filters.shipOwner, 'shipOwner', 'in')
                // this.filterColumn(filters.documentType, 'documentType', 'in')
                // this.filterColumn(filters.invoiceNo, 'invoiceNo', 'contains')
                // this.filterColumn(filters.grossAmount, 'grossAmount', 'contains')
            }, 1000)
        }
    }

    private getVirtualElement(): void {
        this.virtualElement = document.getElementsByClassName('p-scroller-inline')[0]
    }

    private hightlightSavedRow(): void {
        this.helperService.highlightSavedRow(this.feature)
    }

    private initFilteredRecordsCount(): void {
        this.recordsFilteredCount = this.records.length
    }

    private isAnyRowSelected(): boolean {
        return this.selectedRecords.length != 0
    }

    private loadRecords(criteria: ReservationImportListCriteriaVM): Promise<ReservationImportListVM[]> {
        return new Promise((resolve) => {
            this.reservationImportHttpService.getForList(criteria).subscribe(response => {
                this.records = response
                this.helperService.sortArray(this.records, 'date')
                resolve(this.records)
            })
        })
    }

    private populateDropdownFilters(): void {
        this.distinctCustomers = this.helperService.getDistinctRecords(this.records, 'customer', 'description')
        this.distinctDestinations = this.helperService.getDistinctRecords(this.records, 'destination', 'description')
        this.distinctPickupPoints = this.helperService.getDistinctRecords(this.records, 'pickupPoint', 'description')
        this.distinctStatuses = this.helperService.getDistinctRecords(this.records, 'status', 'description')
    }

    private processReservations(x: ReservationImportDto[]): void {
        x.forEach(z => {
            this.reservationImportHttpService.saveReservation(z).subscribe({
                next: (response) => {
                    console.log(response)
                },
                error: (errorFromInterceptor) => {
                    this.dialogService.open(this.messageDialogService.filterResponse(errorFromInterceptor), 'error', ['ok'])
                }
            })
        })
    }

    private scrollToSavedPosition(): void {
        this.helperService.scrollToSavedPosition(this.virtualElement, this.feature)
    }

    private setTabTitle(): void {
        this.helperService.setTabTitle(this.feature)
    }

    private subscribeToInteractionService(): void {
        this.interactionService.refreshTabTitle.subscribe(() => {
            this.setTabTitle()
        })
        this.interactionService.emitDateRange.subscribe((response) => {
            if (response) {
                this.criteria.fromDate = response.fromDate
                this.criteria.toDate = response.toDate
            }
        })
    }

    //#endregion

}
