import { ActivatedRoute, Router } from '@angular/router'
import { Component, ViewChild } from '@angular/core'
import { Table } from 'primeng/table'
// Custom
import { BankAccountListVM } from '../classes/view-models/bankAccount-list-vm'
import { BankListVM } from '../../banks/classes/view-models/bank-list-vm'
import { DialogService } from 'src/app/shared/services/modal-dialog.service'
import { EmojiService } from 'src/app/shared/services/emoji.service'
import { HelperService } from 'src/app/shared/services/helper.service'
import { InteractionService } from 'src/app/shared/services/interaction.service'
import { ListResolved } from 'src/app/shared/classes/list-resolved'
import { MenuItem } from 'primeng/api'
import { MessageDialogService } from 'src/app/shared/services/message-dialog.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { SessionStorageService } from 'src/app/shared/services/session-storage.service'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

@Component({
    selector: 'bankAccount-list',
    templateUrl: './bankAccount-list.component.html',
    styleUrls: ['../../../../../assets/styles/custom/lists.css']
})

export class BankAccountListComponent {

    //#region common

    @ViewChild('table') table: Table

    private url = 'bankAccounts'
    private virtualElement: any
    public feature = 'bankAccountList'
    public featureIcon = 'bankAccounts'
    public icon = 'home'
    public parentUrl = '/home'
    public records: BankAccountListVM[]
    public recordsFilteredCount: number

    //#endregion

    //#region context menu

    public menuItems!: MenuItem[]
    public selectedRecord!: BankListVM

    //#endregion

    //#region dropdown filters

    public distinctBanks: SimpleEntity[] = []
    public distinctShipOwners: SimpleEntity[] = []

    //#endregion

    constructor(private activatedRoute: ActivatedRoute, private dialogService: DialogService, private emojiService: EmojiService, private helperService: HelperService, private interactionService: InteractionService, private messageDialogService: MessageDialogService, private messageLabelService: MessageLabelService, private router: Router, private sessionStorageService: SessionStorageService) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.loadRecords().then(() => {
            this.populateDropdownFilters()
            this.filterTableFromStoredFilters()
            this.subscribeToInteractionService()
            this.setTabTitle()
            this.initContextMenu()
            this.setSidebarsHeight()
        })
    }

    ngAfterViewInit(): void {
        setTimeout(() => {
            this.getVirtualElement()
            this.scrollToSavedPosition()
            this.hightlightSavedRow()
            this.enableDisableFilters()
        }, 500)
    }

    //#endregion

    //#region public methods

    public editRecord(id: number): void {
        this.storeScrollTop()
        this.storeSelectedId(id)
        this.navigateToRecord(id)
    }

    public filterRecords(event: any): void {
        this.sessionStorageService.saveItem(this.feature + '-' + 'filters', JSON.stringify(this.table.filters))
        this.recordsFilteredCount = event.filteredValue.length
    }

    public getEmoji(anything: any): string {
        return typeof anything == 'string'
            ? this.emojiService.getEmoji(anything)
            : anything ? this.emojiService.getEmoji('green-box') : this.emojiService.getEmoji('red-box')
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public highlightRow(id: any): void {
        this.helperService.highlightRow(id)
    }

    private initContextMenu(): void {
        this.menuItems = [
            { label: 'Επεξεργασία', command: () => this.editRecord(this.selectedRecord.id) }
        ]
    }

    public newRecord(): void {
        this.router.navigate([this.url + '/new'])
    }

    public resetTableFilters(): void {
        this.helperService.clearTableTextFilters(this.table, ['description'])
    }

    //#endregion

    //#region private methods

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
                this.filterColumn(filters.isActive, 'isActive', 'contains')
                this.filterColumn(filters.shipOwner, 'shipOwner', 'in')
                this.filterColumn(filters.bank, 'bank', 'in')
                this.filterColumn(filters.iban, 'iban', 'contains')
            }, 500)
        }
    }

    private getVirtualElement(): void {
        this.virtualElement = document.getElementsByClassName('p-scroller-inline')[0]
    }

    private goBack(): void {
        this.router.navigate([this.parentUrl])
    }

    private hightlightSavedRow(): void {
        this.helperService.highlightSavedRow(this.feature)
    }

    private loadRecords(): Promise<any> {
        return new Promise((resolve) => {
            const listResolved: ListResolved = this.activatedRoute.snapshot.data[this.feature]
            if (listResolved.error == null) {
                this.records = listResolved.list
                this.recordsFilteredCount = this.records.length
                resolve(this.records)
            } else {
                this.dialogService.open(this.messageDialogService.filterResponse(listResolved.error), 'error', ['ok']).subscribe(() => {
                    this.goBack()
                })
            }
        })
    }

    private navigateToRecord(id: any): void {
        this.router.navigate([this.url, id])
    }

    private populateDropdownFilters(): void {
        this.distinctBanks = this.helperService.getDistinctRecords(this.records, 'bank', 'description')
        this.distinctShipOwners = this.helperService.getDistinctRecords(this.records, 'shipOwner', 'description')
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

    private storeSelectedId(id: number): void {
        this.sessionStorageService.saveItem(this.feature + '-id', id.toString())
    }

    private storeScrollTop(): void {
        this.sessionStorageService.saveItem(this.feature + '-scrollTop', this.virtualElement.scrollTop)
    }

    private subscribeToInteractionService(): void {
        this.interactionService.refreshTabTitle.subscribe(() => {
            this.setTabTitle()
        })
    }

    //#endregion

}
