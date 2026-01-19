import { ActivatedRoute, Router } from '@angular/router'
import { Component, ViewChild } from '@angular/core'
import { Table } from 'primeng/table'
// Custom
import { DialogService } from 'src/app/shared/services/modal-dialog.service'
import { EmojiService } from 'src/app/shared/services/emoji.service'
import { HelperService } from 'src/app/shared/services/helper.service'
import { InteractionService } from 'src/app/shared/services/interaction.service'
import { ListResolved } from 'src/app/shared/classes/list-resolved'
import { MessageDialogService } from 'src/app/shared/services/message-dialog.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { PickupPointHttpService } from '../classes/services/pickupPoint-http.service'
import { PickupPointListVM } from '../classes/view-models/pickupPoint-list-vm'
import { PickupPointPdfService } from '../classes/services/pickupPoint-pdf.service'
import { SessionStorageService } from 'src/app/shared/services/session-storage.service'

@Component({
    selector: 'pickupPoint-list',
    templateUrl: './pickupPoint-list.component.html',
    styleUrls: ['../../../../../assets/styles/custom/lists.css']
})

export class PickupPointListComponent {

    //#region variables 

    @ViewChild('table') table: Table

    private url = 'pickupPoints'
    private virtualElement: any
    public feature = 'pickupPointList'
    public featureIcon = 'pickupPoints'
    public icon = 'home'
    public parentUrl = '/home'
    public records: PickupPointListVM[]
    public recordsFilteredCount: number
    public recordsFiltered: PickupPointListVM[]

    //#endregion

    //#region dropdown filters

    public dropdownCoachRoutes = []
    public dropdownPorts = []

    //#endregion

    constructor(private activatedRoute: ActivatedRoute, private dialogService: DialogService, private emojiService: EmojiService, private helperService: HelperService, private interactionService: InteractionService, private messageDialogService: MessageDialogService, private messageLabelService: MessageLabelService, private pickupPointHttpService: PickupPointHttpService, private pickupPointPdfService: PickupPointPdfService, private router: Router, private sessionStorageService: SessionStorageService) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.loadRecords().then(() => {
            this.populateDropdownFilters()
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

    //#region public methods

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

    public onCreatePdf(): void {
        this.pickupPointPdfService.createReport(this.recordsFiltered)
    }

    public onEditRecord(id: number): void {
        this.storeScrollTop()
        this.storeSelectedId(id)
        this.navigateToRecord(id)
    }

    public onFilterRecords(event: any): void {
        this.sessionStorageService.saveItem(this.feature + '-' + 'filters', JSON.stringify(this.table.filters))
        this.recordsFiltered = event.filteredValue
        this.recordsFilteredCount = event.filteredValue.length
    }

    public onUpdateFromLinkTwist(): void {
        this.dialogService.open(this.messageDialogService.confirmUpdatePickupPointsFromLinkTwist(), 'question', ['abort', 'ok']).subscribe(response => {
            if (response) {
                this.pickupPointHttpService.getFromLinkTwist().subscribe({
                    next: (response: any) => {
                        this.seekRecord(response)
                    },
                    error: (errorFromInterceptor) => {
                        this.dialogService.open(this.messageDialogService.filterResponse(errorFromInterceptor), 'error', ['ok'])
                    }
                })
            }
        })
    }

    public onNewRecord(): void {
        this.router.navigate([this.url + '/new'])
    }

    public onResetTableFilters(): void {
        this.helperService.clearTableTextFilters(this.table)
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
                this.filterColumn(filters.coachRoute, 'coachRoute', 'in')
                this.filterColumn(filters.port, 'port', 'in')
                this.filterColumn(filters.description, 'description', 'contains')
                this.filterColumn(filters.linkTwistAlias, 'linkTwistAlias', 'contains')
                this.filterColumn(filters.exactPoint, 'exactPoint', 'contains')
                this.filterColumn(filters.time, 'time', 'contains')
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
                this.recordsFiltered = listResolved.list
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
        this.dropdownCoachRoutes = this.helperService.getDistinctRecords(this.records, 'coachRoute', 'abbreviation')
        this.dropdownPorts = this.helperService.getDistinctRecords(this.records, 'port', 'abbreviation')
    }

    private scrollToSavedPosition(): void {
        this.helperService.scrollToSavedPosition(this.virtualElement, this.feature)
    }

    private seekRecord(response: any[]): void {
        response.forEach(pickupPoint => {
            const linkTwist = pickupPoint.title.split('|')[0].split('-')[0].trim()
            const ourRecord: PickupPointListVM = this.records.find(x => x.description == linkTwist)
            if (ourRecord) {
                if (linkTwist == ourRecord.description) {
                    // console.log(linkTwist + ' ' + ourRecord.description)
                } else {
                    console.log(linkTwist + ' ' + 'not found in our table')
                }
            } else {
                console.log(JSON.stringify(pickupPoint) + ' ' + 'not found in our table')
            }
        })
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
