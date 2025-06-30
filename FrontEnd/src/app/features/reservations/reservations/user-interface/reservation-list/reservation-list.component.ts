import { ActivatedRoute, Router } from '@angular/router'
import { Component, ViewChild } from '@angular/core'
import { MatDialog } from '@angular/material/dialog'
import { Subscription } from 'rxjs'
import { Table } from 'primeng/table'
// Custom
import { CryptoService } from 'src/app/shared/services/crypto.service'
import { DateHelperService } from 'src/app/shared/services/date-helper.service'
import { DeleteRangeDialogComponent } from 'src/app/shared/components/delete-range-dialog/delete-range-dialog.component'
import { DialogService } from 'src/app/shared/services/modal-dialog.service'
import { DriverReportService } from '../../classes/driver-report/services/driver-report.service'
import { EmojiService } from './../../../../../shared/services/emoji.service'
import { HelperService } from './../../../../../shared/services/helper.service'
import { InteractionService } from 'src/app/shared/services/interaction.service'
import { ListResolved } from 'src/app/shared/classes/list-resolved'
import { MenuItem } from 'primeng/api'
import { MessageDialogService } from 'src/app/shared/services/message-dialog.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { ReservationAssignDialogComponent } from '../reservation-assign-dialog/reservation-assign-dialog.component'
import { ReservationHttpService } from '../../classes/services/reservation.http.service'
import { ReservationListDestinationVM } from '../../classes/view-models/list/reservation-list-destination-vm'
import { ReservationListOverbookedDestinationVM } from '../../classes/view-models/list/reservation-list-overbooked-destination-vm'
import { ReservationListVM } from '../../classes/view-models/list/reservation-list-vm'
import { ReservationReportPdfService } from '../../classes/reservation-report/services/reservation-report.service'
import { SessionStorageService } from 'src/app/shared/services/session-storage.service'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

@Component({
    selector: 'reservation-list',
    templateUrl: './reservation-list.component.html',
    styleUrls: ['../../../../../../assets/styles/custom/lists.css']
})

export class ReservationListComponent {

    //#region variables

    @ViewChild('table') table: Table | undefined

    private subscription = new Subscription()
    private virtualElement: any
    public feature = 'reservationList'
    public featureIcon = 'reservations'
    public icon = 'arrow_back'
    public parentUrl = '/reservations'
    public records: ReservationListVM[] = []

    public overbookedDestinations: ReservationListOverbookedDestinationVM[] = []
    public selectedRecords: ReservationListVM[] = []
    public selectedIds: string[] = []
    public totalPax = [0, 0, 0]

    public distinctCoachRoutes: SimpleEntity[] = []
    public distinctCustomers: SimpleEntity[] = []
    public distinctDestinations: ReservationListDestinationVM[] = []
    public distinctDrivers: SimpleEntity[] = []
    public distinctPickupPoints: SimpleEntity[] = []
    public distinctPorts: SimpleEntity[] = []
    public distinctPortsAlternate: SimpleEntity[] = []
    public distinctShips: SimpleEntity[] = []

    //#endregion

    //#region context menu

    public menuItems!: MenuItem[]
    public selectedRecord!: ReservationListVM

    //#endregion

    constructor(private activatedRoute: ActivatedRoute, private cryptoService: CryptoService, private dateHelperService: DateHelperService, private dialogService: DialogService, private driverReportService: DriverReportService, private emojiService: EmojiService, private helperService: HelperService, private interactionService: InteractionService, private messageDialogService: MessageDialogService, private messageLabelService: MessageLabelService, private reservationHttpService: ReservationHttpService, private reservationReportPdfService: ReservationReportPdfService, private router: Router, private sessionStorageService: SessionStorageService, public dialog: MatDialog) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.loadRecords()
        this.populateDropdownFilters()
        this.filterTableFromStoredFilters()
        this.updateTotals(this.totalPax, this.records)
        this.calculateOverbookings()
        this.enableDisableFilters()
        this.storeCriteria()
        this.subscribeToInteractionService()
        this.setTabTitle()
        this.doVirtualTableTasks()
        this.setSidebarsHeight()
        this.initContextMenu()
    }

    ngOnDestroy(): void {
        this.cleanup()
    }

    //#endregion

    //#region public methods

    public assignToDriver(): void {
        if (this.isAnyRowSelected()) {
            this.saveSelectedIds()
            const dialogRef = this.dialog.open(ReservationAssignDialogComponent, {
                data: ['drivers', 'assignToDriver'],
                height: '36.0625rem',
                panelClass: 'dialog',
                width: '32rem',
            })
            dialogRef.afterClosed().subscribe(result => {
                if (result !== undefined) {
                    this.reservationHttpService.assignToDriver(result.record.id, this.selectedIds).subscribe(() => {
                        this.dialogService.open(this.messageDialogService.success(), 'ok', ['ok']).subscribe(() => {
                            this.clearSelectedRecords()
                            this.resetTableFilters()
                            this.refreshList()
                        })
                    })
                }
            })
        }
    }

    public assignToPort(): void {
        if (this.isAnyRowSelected()) {
            this.saveSelectedIds()
            const dialogRef = this.dialog.open(ReservationAssignDialogComponent, {
                data: ['ports', 'assignToPort'],
                height: '36.0625rem',
                panelClass: 'dialog',
                width: '32rem',
            })
            dialogRef.afterClosed().subscribe(result => {
                if (result !== undefined) {
                    this.reservationHttpService.assignToPort(result.record.id, this.selectedIds).subscribe(() => {
                        this.dialogService.open(this.messageDialogService.success(), 'ok', ['ok']).subscribe(() => {
                            this.clearSelectedRecords()
                            this.resetTableFilters()
                            this.refreshList()
                        })
                    })
                }
            })
        }
    }

    public assignToShip(): void {
        if (this.isAnyRowSelected()) {
            this.saveSelectedIds()
            const dialogRef = this.dialog.open(ReservationAssignDialogComponent, {
                data: ['ships', 'assignToShip'],
                height: '36.0625rem',
                panelClass: 'dialog',
                width: '32rem',
            })
            dialogRef.afterClosed().subscribe(result => {
                if (result !== undefined) {
                    this.reservationHttpService.assignToShip(result.record.id, this.selectedIds).subscribe(() => {
                        this.dialogService.open(this.messageDialogService.success(), 'ok', ['ok']).subscribe(() => {
                            this.clearSelectedRecords()
                            this.resetTableFilters()
                            this.refreshList()
                        })
                    })
                }
            })
        }
    }

    public calculateSelectedPax(): void {
        this.totalPax[2] = this.selectedRecords.reduce((sum, array) => sum + array.totalPax, 0)
    }

    public createPdf(): void {
        this.driverReportService.doReportTasks(this.getDistinctDriverIds())
    }

    public createReservationsPdf(): void {
        if (this.isAnyRowSelected()) {
            this.reservationReportPdfService.createReport(this.selectedRecords, this.sessionStorageService.getItem('date'))
        }
    }

    public reservationYearIsSelectedYear(date: string): boolean {
        return this.dateHelperService.createDateFromString(date).getFullYear() == new Date().getFullYear()
    }

    public deleteRange(): void {
        if (this.isAnyRowSelected()) {
            const dialogRef = this.dialog.open(DeleteRangeDialogComponent, {
                data: 'question',
                panelClass: 'dialog',
                height: '18.75rem',
                width: '31.25rem'
            })
            dialogRef.afterClosed().subscribe(result => {
                if (result != undefined) {
                    this.saveSelectedIds()
                    this.reservationHttpService.deleteRange(this.selectedIds).subscribe(() => {
                        this.dialogService.open(this.messageDialogService.success(), 'ok', ['ok']).subscribe(() => {
                            this.clearSelectedRecords()
                            this.resetTableFilters()
                            this.refreshList()
                        })
                    })
                }
            })
        }
    }

    public filterRecords(event: any): void {
        this.helperService.clearTableCheckboxes()
        this.sessionStorageService.saveItem(this.feature + '-' + 'filters', JSON.stringify(this.table.filters))
        this.selectedRecords.splice(0)
        this.updateTotals(this.totalPax, event.filteredValue)
    }

    public formatDateToLocale(date: string, showWeekday = false, showYear = false): string {
        return this.dateHelperService.formatISODateToLocale(date, showWeekday, showYear)
    }

    public getDateFromStorage(): string {
        const date = this.sessionStorageService.getItem('date')
        if (date != '') {
            return date
        } else {
            return this.dateHelperService.formatDateToIso(new Date())
        }
    }

    public getEmoji(emoji: string): string {
        return this.emojiService.getEmoji(emoji)
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public isAdmin(): boolean {
        return this.cryptoService.decrypt(this.sessionStorageService.getItem('isAdmin')) == 'true' ? true : false
    }

    public isFilterDisabled(): boolean {
        return this.records.length == 0
    }

    public isSearchByRefNo(): boolean {
        return this.sessionStorageService.getItem('isSearchByRefNo') ? true : false
    }

    public onEditRecord(id: string): void {
        this.storeScrollTop()
        this.storeSelectedId(id)
        this.gotoEditForm(id)
        this.storeUrl()
    }

    public onHighlightRow(id: any): void {
        this.helperService.highlightRow(id)
    }

    public newRecord(): void {
        this.sessionStorageService.deleteItems([{ 'item': 'isSearchByRefNo', 'when': 'always' }])
        this.router.navigate([this.parentUrl, 'new'])
    }

    public resetTableFilters(): void {
        this.helperService.clearTableTextFilters(this.table, ['refNo', 'ticketNo'])
        this.selectedRecords = []
        this.totalPax[1] = this.totalPax[0]
        this.totalPax[2] = 0
    }

    public showPassengerDifferenceEmoji(passengerDifference: number): string {
        if (passengerDifference > 0) {
            return this.emojiService.getEmoji('yellow-box')
        }
        if (passengerDifference == 0) {
            return this.emojiService.getEmoji('green-box')
        }
        if (passengerDifference < 0) {
            return this.emojiService.getEmoji('red-box')
        }
    }

    public showDifferentPortsEmoji(reservation: ReservationListVM): string {
        if (reservation.port.id == reservation.portAlternate.id) {
            return ''
        } else {
            return this.emojiService.getEmoji('blue-box')
        }
    }

    //#endregion

    //#region private methods

    private calculateOverbookings(): void {
        const date = this.sessionStorageService.getItem('date')
        if (date != '') {
            this.overbookedDestinations = []
            this.distinctDestinations.forEach((destination) => {
                this.reservationHttpService.isDestinationOverbooked(this.sessionStorageService.getItem('date'), destination.id).subscribe((response) => {
                    this.overbookedDestinations.push({
                        description: destination.abbreviation,
                        overbookedPax: response
                    })
                })
            })
        }
    }

    private clearSelectedRecords(): void {
        this.selectedRecords = []
    }

    private cleanup(): void {
        this.subscription.unsubscribe()
    }

    private doVirtualTableTasks(): void {
        setTimeout(() => {
            this.getVirtualElement()
            this.scrollToSavedPosition()
            this.hightlightSavedRow()
        }, 1000)
    }

    private enableDisableFilters(): void {
        this.records.length == 0 ? this.helperService.disableTableFilters() : this.helperService.enableTableFilters()
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
                this.filterColumn(filters.refNo, 'refNo', 'contains')
                this.filterColumn(filters.ticketNo, 'ticketNo', 'contains')
                this.filterColumn(filters.customer, 'customer', 'in')
                this.filterColumn(filters.destination, 'destination', 'in')
                this.filterColumn(filters.coachRoute, 'coachRoute', 'in')
                this.filterColumn(filters.pickupPoint, 'pickupPoint', 'in')
                this.filterColumn(filters.driver, 'driver', 'in')
                this.filterColumn(filters.port, 'port', 'in')
                this.filterColumn(filters.ship, 'ship', 'in')
            }, 500)
        }
    }

    private getDistinctDriverIds(): any[] {
        const driverIds = []
        this.distinctDrivers.forEach(driver => {
            driverIds.push(driver.id)
        })
        return driverIds
    }

    private getVirtualElement(): void {
        this.virtualElement = document.getElementsByClassName('p-scroller-inline')[0]
    }

    private gotoEditForm(id: any): void {
        this.router.navigate([this.parentUrl, id])
    }

    private hightlightSavedRow(): void {
        this.helperService.highlightSavedRow(this.feature)
    }

    private initContextMenu(): void {
        this.menuItems = [
            { label: this.getLabel('contextMenuEdit'), command: () => this.onEditRecord(this.selectedRecord.reservationId) },
            { label: this.getLabel('contextMenuCopy'), command: () => this.helperService.copyToClipboard() },
        ]
    }

    private isAnyRowSelected(): boolean {
        if (this.selectedRecords.length == 0) {
            this.dialogService.open(this.messageDialogService.noRecordsSelected(), 'error', ['ok'])
            return false
        }
        return true
    }

    private loadRecords(): Promise<any> {
        return new Promise((resolve) => {
            const listResolved: ListResolved = this.activatedRoute.snapshot.data[this.feature]
            if (listResolved.error === null) {
                this.records = listResolved.list
                resolve(this.records)
            } else {
                this.dialogService.open(this.messageDialogService.filterResponse(listResolved.error), 'error', ['ok']).subscribe(() => {
                    this.router.navigate([this.parentUrl])
                })
            }
        })
    }

    private populateDropdownFilters(): void {
        this.distinctCoachRoutes = this.helperService.getDistinctRecords(this.records, 'coachRoute', 'abbreviation')
        this.distinctCustomers = this.helperService.getDistinctRecords(this.records, 'customer', 'description')
        this.distinctDestinations = this.helperService.getDistinctRecords(this.records, 'destination', 'description')
        this.distinctDrivers = this.helperService.getDistinctRecords(this.records, 'driver', 'description')
        this.distinctPickupPoints = this.helperService.getDistinctRecords(this.records, 'pickupPoint', 'description')
        this.distinctPorts = this.helperService.getDistinctRecords(this.records, 'port', 'description')
        this.distinctPortsAlternate = this.helperService.getDistinctRecords(this.records, 'portAlternate', 'description')
        this.distinctShips = this.helperService.getDistinctRecords(this.records, 'ship', 'description')
    }

    private refreshList(): void {
        this.router.navigateByUrl(this.router.url)
    }

    private saveSelectedIds(): void {
        const ids = []
        this.selectedRecords.forEach(record => {
            ids.push(record.reservationId)
        })
        this.selectedIds = ids
    }

    private scrollToSavedPosition(): void {
        this.helperService.scrollToSavedPosition(this.virtualElement, this.feature)
    }

    private setSidebarsHeight(): void {
        this.helperService.setSidebarsTopMargin('0')
    }

    private setTabTitle(): void {
        this.helperService.setTabTitle(this.feature)
    }

    private storeCriteria(): void {
        this.sessionStorageService.saveItem('returnUrl', '/reservations')
    }

    private storeScrollTop(): void {
        this.sessionStorageService.saveItem(this.feature + '-scrollTop', this.virtualElement.scrollTop)
    }

    private storeSelectedId(id: string): void {
        this.sessionStorageService.saveItem(this.feature + '-id', id)
    }

    private storeUrl(): void {
        this.activatedRoute.params.subscribe(x => {
            if (x.refNo) {
                this.sessionStorageService.saveItem('returnUrl', '/reservations/refNo/' + x.refNo)
            }
        })
    }

    private subscribeToInteractionService(): void {
        this.interactionService.refreshTabTitle.subscribe(() => {
            this.setTabTitle()
        })
    }

    private updateTotals(totalPax: number[], filteredValue: any[]): void {
        totalPax[0] = this.records.reduce((sum: number, array: { totalPax: number }) => sum + array.totalPax, 0)
        totalPax[1] = filteredValue.reduce((sum: number, array: { totalPax: number }) => sum + array.totalPax, 0)
        totalPax[2] = this.selectedRecords.reduce((sum: number, array: { totalPax: number }) => sum + array.totalPax, 0)
    }

    //#endregion

}
