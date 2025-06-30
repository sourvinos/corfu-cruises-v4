import { Component } from '@angular/core'
import { MatDialog } from '@angular/material/dialog'
// Custom
import { BalanceSheetCriteriaVM } from '../classes/view-models/criteria/balanceSheet-criteria-vm'
import { BalanceSheetExportService } from '../classes/services/balanceSheet-export.service'
import { BalanceSheetHttpService } from '../classes/services/balanceSheet-http.service'
import { BalanceSheetVM } from '../classes/view-models/list/balanceSheet-vm'
import { CriteriaDateRangeDialogComponent } from 'src/app/shared/components/criteria-date-range-dialog/criteria-date-range-dialog.component'
import { DateHelperService } from 'src/app/shared/services/date-helper.service'
import { HelperService } from '../../../../shared/services/helper.service'
import { InteractionService } from 'src/app/shared/services/interaction.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'

@Component({
    selector: 'balanceSheet',
    templateUrl: './balanceSheet-parent.component.html',
    styleUrls: ['../../../../../assets/styles/custom/lists.css', './balanceSheet-parent.component.css']
})

export class BalanceSheetParentComponent {

    //#region variables

    public criteria: BalanceSheetCriteriaVM
    public feature = 'balanceSheet'
    public featureIcon = 'balanceSheet'
    public parentUrl = '/home'
    public shipOwnerRecordsA: BalanceSheetVM[] = []
    public shipOwnerFilteredRecordsA: BalanceSheetVM[] = []
    public shipOwnerRecordsB: BalanceSheetVM[] = []
    public shipOwnerFilteredRecordsB: BalanceSheetVM[] = []
    public shipOwnerTotal: BalanceSheetVM[] = []
    public shipOwnerFilteredTotal: BalanceSheetVM[] = []
    public showZeroBalanceRow: boolean = true

    //#endregion

    constructor(private balanceSheetExportService: BalanceSheetExportService, private balanceSheetHttpService: BalanceSheetHttpService, private dateHelperService: DateHelperService, private helperService: HelperService, private interactionService: InteractionService, private messageLabelService: MessageLabelService, public dialog: MatDialog) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.setTabTitle()
        this.subscribeToInteractionService()
        this.setListHeight()
    }

    //#endregion

    //#region public methods

    public getCriteria(): string {
        return this.criteria
            ? this.dateHelperService.formatISODateToLocale(this.criteria.fromDate) + ' - ' + this.dateHelperService.formatISODateToLocale(this.criteria.toDate)
            : ''
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public onExportTasks(): void {
        const x = this.balanceSheetExportService.buildVM(this.shipOwnerFilteredRecordsA)
        const z = this.balanceSheetExportService.buildVM(this.shipOwnerFilteredRecordsB)
        const i = this.balanceSheetExportService.buildVM(this.shipOwnerFilteredTotal)
        this.balanceSheetExportService.exportToExcel(x, z, i)
    }

    public onSelectedTabChange(): void {
        setTimeout(() => {
            const x = document.getElementsByClassName('table-wrapper') as HTMLCollectionOf<HTMLInputElement>
            for (let i = 0; i < x.length; i++) {
                x[i].style.height = document.getElementById('content').offsetHeight - 150 + 'px'
            }
        }, 100)
    }

    public onShowCriteriaDialog(): void {
        const dialogRef = this.dialog.open(CriteriaDateRangeDialogComponent, {
            data: 'balanceSheetCriteria',
            height: '36.0625rem',
            panelClass: 'dialog',
            width: '32rem',
        })
        dialogRef.afterClosed().subscribe(criteria => {
            if (criteria !== undefined) {
                this.initVariables()
                this.buildCriteriaVM(criteria)
                this.loadRecordsForShipOwner(this.criteria, 'shipOwnerRecordsA', 'shipOwnerFilteredRecordsA', 1)
                this.loadRecordsForShipOwner(this.criteria, 'shipOwnerRecordsB', 'shipOwnerFilteredRecordsB', 2)
                this.loadRecordsForShipOwner(this.criteria, 'shipOwnerTotal', 'shipOwnerFilteredTotal', null)
            }
        })
    }

    public onToggleZeroBalanceRows(): void {
        this.toggleZeroBalanceRecords()
    }

    //#endregion

    //#region private methods

    private buildCriteriaVM(event: BalanceSheetCriteriaVM): void {
        this.criteria = {
            fromDate: event.fromDate,
            toDate: event.toDate,
            shipOwnerId: event.shipOwnerId
        }
    }

    private loadRecordsForShipOwner(criteria: BalanceSheetCriteriaVM, shipOwnerRecords: string, shipOwnerFilteredRecords: string, shipOwnerId: number): void {
        const x: BalanceSheetCriteriaVM = {
            fromDate: criteria.fromDate,
            toDate: criteria.toDate,
            shipOwnerId: shipOwnerId
        }
        this.balanceSheetHttpService.get(x).subscribe(response => {
            this[shipOwnerRecords] = response
            this[shipOwnerFilteredRecords] = response
        })
    }

    private initVariables(): void {
        this.showZeroBalanceRow = true
    }

    private setListHeight(): void {
        setTimeout(() => {
            document.getElementById('content').style.height = document.getElementById('list-wrapper').offsetHeight - 64 + 'px'
        }, 100)
    }

    private setTabTitle(): void {
        this.helperService.setTabTitle(this.feature)
    }

    private subscribeToInteractionService(): void {
        this.interactionService.refreshTabTitle.subscribe(() => { this.setTabTitle() })
        this.interactionService.emitDateRange.subscribe((response) => {
            if (response) {
                this.criteria.fromDate = response.fromDate
                this.criteria.toDate = response.toDate
            }
        })
    }

    private toggleZeroBalanceRecords(): void {
        if (this.showZeroBalanceRow) {
            this.shipOwnerFilteredRecordsA = this.shipOwnerRecordsA
            this.shipOwnerFilteredRecordsB = this.shipOwnerRecordsB
            this.shipOwnerFilteredTotal = this.shipOwnerTotal
        } else {
            this.shipOwnerFilteredRecordsA = this.shipOwnerRecordsA.filter(x => x.actualBalance != 0)
            this.shipOwnerFilteredRecordsB = this.shipOwnerRecordsB.filter(x => x.actualBalance != 0)
            this.shipOwnerFilteredTotal = this.shipOwnerTotal.filter(x => x.actualBalance != 0)
        }
    }

    //#endregion

}
