import { ActivatedRoute, Router } from '@angular/router'
import { Component, ViewChild } from '@angular/core'
import { Table } from 'primeng/table'
// Custom
import { DialogService } from 'src/app/shared/services/modal-dialog.service'
import { DriverListVM } from '../classes/view-models/driver-list-vm'
import { EmojiService } from 'src/app/shared/services/emoji.service'
import { HelperService } from 'src/app/shared/services/helper.service'
import { InteractionService } from 'src/app/shared/services/interaction.service'
import { ListResolved } from 'src/app/shared/classes/list-resolved'
import { MessageDialogService } from 'src/app/shared/services/message-dialog.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { SessionStorageService } from 'src/app/shared/services/session-storage.service'

@Component({
    selector: 'driver-list',
    templateUrl: './driver-list.component.html',
    styleUrls: ['../../../../../assets/styles/custom/lists.css']
})

export class DriverListComponent {

    //#region common #9

    @ViewChild('table') table: Table

    private url = 'drivers'
    private virtualElement: any
    public feature = 'driverList'
    public featureIcon = 'drivers'
    public icon = 'home'
    public parentUrl = '/home'
    public records: DriverListVM[]
    public recordsFilteredCount: number

    //#endregion

    constructor(private activatedRoute: ActivatedRoute, private dialogService: DialogService, private emojiService: EmojiService, private helperService: HelperService, private interactionService: InteractionService, private messageDialogService: MessageDialogService, private messageLabelService: MessageLabelService, private router: Router, private sessionStorageService: SessionStorageService) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.loadRecords().then(() => {
            this.filterTableFromStoredFilters()
            this.subscribeToInteractionService()
            this.setTabTitle()
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

    //#region public common methods #7

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

    public newRecord(): void {
        this.router.navigate([this.url + '/new'])
    }

    public resetTableFilters(): void {
        this.helperService.clearTableTextFilters(this.table, ['description', 'phones'])
    }

    //#endregion

    //#region private common methods #13

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
                this.filterColumn(filters.description, 'description', 'contains')
                this.filterColumn(filters.phones, 'phones', 'contains')
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
