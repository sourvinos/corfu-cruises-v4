import { ActivatedRoute, Router } from '@angular/router'
import { Component, ViewChild } from '@angular/core'
import { DateAdapter } from '@angular/material/core'
import { MenuItem } from 'primeng/api'
import { Table } from 'primeng/table'
// Custom
import { DialogService } from 'src/app/shared/services/modal-dialog.service'
import { DocumentTypeListVM } from '../classes/view-models/documentType-list-vm'
import { EmojiService } from 'src/app/shared/services/emoji.service'
import { HelperService } from 'src/app/shared/services/helper.service'
import { InteractionService } from 'src/app/shared/services/interaction.service'
import { ListResolved } from '../../../../shared/classes/list-resolved'
import { LocalStorageService } from 'src/app/shared/services/local-storage.service'
import { MessageDialogService } from 'src/app/shared/services/message-dialog.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { SessionStorageService } from 'src/app/shared/services/session-storage.service'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

@Component({
    selector: 'documentType-list',
    templateUrl: './documentType-list.component.html',
    styleUrls: ['../../../../../assets/styles/custom/lists.css']
})

export class DocumentTypeListComponent {

    //#region variables

    @ViewChild('table') table: Table

    private url = 'documentTypes'
    private virtualElement: any
    public feature = 'documentTypeList'
    public featureIcon = 'documentTypes'
    public icon = 'home'
    public parentUrl = '/home'
    public records: DocumentTypeListVM[]
    public recordsFilteredCount: number

    //#endregion

    //#region context menu

    public menuItems!: MenuItem[]
    public selectedRecord!: DocumentTypeListVM

    //#endregion

    //#region dropdown filters

    public distinctShips: SimpleEntity[] = []
    public distinctShipOwners: SimpleEntity[] = []

    //#endregion

    constructor(private activatedRoute: ActivatedRoute, private dateAdapter: DateAdapter<any>, private dialogService: DialogService, private emojiService: EmojiService, private helperService: HelperService, private interactionService: InteractionService, private localStorageService: LocalStorageService, private messageDialogService: MessageDialogService, private messageLabelService: MessageLabelService, private router: Router, private sessionStorageService: SessionStorageService) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.loadRecords().then(() => {
            this.populateDropdownFilters()
            this.filterTableFromStoredFilters()
            this.subscribeToInteractionService()
            this.setTabTitle()
            this.setLocale()
            this.setSidebarsHeight()
            this.initContextMenu()
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

    public onEditRecord(id: number): void {
        this.storeScrollTop()
        this.storeSelectedId(id)
        this.navigateToRecord(id)
    }

    public onFilterRecords(event: any): void {
        this.sessionStorageService.saveItem(this.feature + '-' + 'filters', JSON.stringify(this.table.filters))
        this.recordsFilteredCount = event.filteredValue.length
    }

    public onHighlightRow(id: any): void {
        this.helperService.highlightRow(id)
    }

    public onNewRecord(): void {
        this.router.navigate([this.url + '/new'])
    }

    public onResetTableFilters(): void {
        this.helperService.clearTableTextFilters(this.table, ['description', 'email', 'phones'])
    }

    public getEmoji(anything: any): string {
        return typeof anything == 'string'
            ? this.emojiService.getEmoji(anything)
            : anything ? this.emojiService.getEmoji('green-box') : this.emojiService.getEmoji('red-box')
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
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
                this.filterColumn(filters.shipOwner, 'shipOwner', 'contains')
                this.filterColumn(filters.ship, 'ship', 'contains')
                this.filterColumn(filters.description, 'description', 'contains')
                this.filterColumn(filters.batch, 'batch', 'contains')
                this.filterColumn(filters.table8_1, 'table8_1', 'contains')
                this.filterColumn(filters.table8_8, 'table8_8', 'contains')
                this.filterColumn(filters.table8_9, 'table8_9', 'contains')
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

    private initContextMenu(): void {
        this.menuItems = [
            { label: 'Επεξεργασία', command: () => this.onEditRecord(this.selectedRecord.id) }
        ]
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

    //#region specific methods

    private populateDropdownFilters(): void {
        this.distinctShips = this.helperService.getDistinctRecords(this.records, 'ship', 'description')
        this.distinctShipOwners = this.helperService.getDistinctRecords(this.records, 'shipOwner', 'description')
    }

    private setLocale(): void {
        this.dateAdapter.setLocale(this.localStorageService.getLanguage())
    }

    //#endregion

}
