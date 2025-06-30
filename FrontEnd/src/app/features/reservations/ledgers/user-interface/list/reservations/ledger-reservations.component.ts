import { Component, EventEmitter, Input, Output } from '@angular/core'
// Custom
import { DateHelperService } from 'src/app/shared/services/date-helper.service'
import { DialogService } from 'src/app/shared/services/modal-dialog.service'
import { EmojiService } from 'src/app/shared/services/emoji.service'
import { HelperService } from 'src/app/shared/services/helper.service'
import { LedgerVM } from '../../../classes/view-models/list/ledger-vm'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { LedgerReservationVM } from '../../../classes/view-models/list/ledger-reservation-vm'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

@Component({
    selector: 'ledger-customer-reservations',
    templateUrl: './ledger-reservations.component.html',
    styleUrls: ['./ledger-reservations.component.css']
})

export class LedgerCustomerReservationListComponent {

    //#region variables

    @Input() customer: LedgerVM
    @Input() remarksRowVisibility: boolean
    @Output() outputSelected = new EventEmitter()

    public selectedRecords: LedgerReservationVM[] = []
    private feature = 'ledgerList'
    private perPort: PerPort[] = []

    //#endregion

    constructor(private dateHelperService: DateHelperService, private dialogService: DialogService, private emojiService: EmojiService, private helperService: HelperService, private messageLabelService: MessageLabelService) { }

    //#region public methods

    public formatDateToLocale(date: string, showWeekday = false, showYear = false): string {
        return this.dateHelperService.formatISODateToLocale(date, showWeekday, showYear)
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

    public hasRemarks(remarks: string): boolean {
        return remarks.length > 0 ? true : false
    }

    public highlightRow(id: any): void {
        this.helperService.highlightRow(id)
    }

    public showRemarks(remarks: string): void {
        this.dialogService.open(remarks, 'info', ['ok'])
    }

    public onValidateSelection(): void {
        if (this.selectedRecords.length >= 1) {
            if (this.isSameShipSelected() && this.isSameDestinationSelected()) {
                this.initPortsArray()
                this.populatePorts()
                this.outputSelected.emit(this.perPort)
            } else {
                this.perPort = []
                this.outputSelected.emit(this.perPort)
            }
        } else {
            this.perPort = []
            this.outputSelected.emit(this.perPort)
        }
    }

    //#endregion

    //#region private methods

    private initPortsArray(): void {
        this.perPort = [
            {
                date: this.selectedRecords[0].date,
                customer: {
                    id: this.customer.customer.id,
                    description: this.customer.customer.description,
                    isActive: this.customer.customer.isActive
                },
                destination: {
                    id: this.selectedRecords[0].destination.id,
                    description: this.selectedRecords[0].destination.description,
                    isActive: true
                },
                port: {
                    id: 1,
                    description: 'CP',
                    isActive: true
                },
                ship: {
                    id: this.selectedRecords[0].ship.id,
                    description: this.selectedRecords[0].ship.description,
                    isActive: true
                },
                adultsWithTransfer: 0,
                adultsWithoutTransfer: 0,
                kidsWithTransfer: 0,
                kidsWithoutTransfer: 0,
                freeWithTransfer: 0,
                freeWithoutTransfer: 0,
                total: 0
            },
            {
                date: this.selectedRecords[0].date,
                customer: {
                    id: this.customer.customer.id,
                    description: this.customer.customer.description,
                    isActive: this.customer.customer.isActive
                },
                destination: {
                    id: this.selectedRecords[0].destination.id,
                    description: this.selectedRecords[0].destination.description,
                    isActive: true
                },
                port: {
                    id: 2,
                    description: 'LP',
                    isActive: true
                },
                ship: {
                    id: this.selectedRecords[0].ship.id,
                    description: this.selectedRecords[0].ship.description,
                    isActive: true
                },
                adultsWithTransfer: 0,
                adultsWithoutTransfer: 0,
                kidsWithTransfer: 0,
                kidsWithoutTransfer: 0,
                freeWithTransfer: 0,
                freeWithoutTransfer: 0,
                total: 0
            }
        ]
    }

    private populatePorts(): void {
        this.selectedRecords.forEach(record => {
            if (record.port.id == 1) {
                if (record.hasTransfer) {
                    this.perPort[0].adultsWithTransfer += record.adults
                    this.perPort[0].kidsWithTransfer += record.kids
                    this.perPort[0].freeWithTransfer += record.free
                } else {
                    this.perPort[0].adultsWithoutTransfer += record.adults
                    this.perPort[0].kidsWithoutTransfer += record.kids
                    this.perPort[0].freeWithoutTransfer += record.free
                }
                this.perPort[0].total += record.adults + record.kids + record.free
            }
            if (record.port.id == 2) {
                if (record.hasTransfer) {
                    this.perPort[1].adultsWithTransfer += record.adults
                    this.perPort[1].kidsWithTransfer += record.kids
                    this.perPort[1].freeWithTransfer += record.free
                } else {
                    this.perPort[1].adultsWithoutTransfer += record.adults
                    this.perPort[1].kidsWithoutTransfer += record.kids
                    this.perPort[1].freeWithoutTransfer += record.free
                }
                this.perPort[1].total += record.adults + record.kids + record.free
            }
        })
    }

    private isSameShipSelected(): boolean {
        return this.helperService.getDistinctRecords(this.selectedRecords, 'ship', 'description').length == 1
    }

    private isSameDestinationSelected(): boolean {
        return this.helperService.getDistinctRecords(this.selectedRecords, 'destination', 'description').length == 1
    }

    //#endregion

}

export interface PerPort {

    date: string
    customer: SimpleEntity
    destination: SimpleEntity
    port: SimpleEntity
    ship: SimpleEntity
    adultsWithTransfer: number
    adultsWithoutTransfer: number
    kidsWithTransfer: number
    kidsWithoutTransfer: number
    freeWithTransfer: 0
    freeWithoutTransfer: 0
    total: 0

}
