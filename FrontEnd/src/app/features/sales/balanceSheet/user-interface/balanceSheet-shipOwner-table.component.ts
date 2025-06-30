import { Component, Input, ViewChild } from '@angular/core'
import { Table } from 'primeng/table'
import { formatNumber } from '@angular/common'
// Custom
import { BalanceSheetCriteriaVM } from '../classes/view-models/criteria/balanceSheet-criteria-vm'
import { BalanceSheetVM } from '../classes/view-models/list/balanceSheet-vm'
import { LocalStorageService } from 'src/app/shared/services/local-storage.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'

@Component({
    selector: 'balanceSheetShipOwnerTable',
    templateUrl: './balanceSheet-shipOwner-table.component.html',
    styleUrls: ['../../../../../assets/styles/custom/lists.css']
})

export class BalanceSheetShipOwnerTableComponent {

    //#region variables

    @ViewChild('table') table: Table
    
    @Input() records: BalanceSheetVM[]
    @Input() criteria: BalanceSheetCriteriaVM

    public feature = 'balanceSheet'
    public totals: BalanceSheetVM

    //#endregion

    constructor(private localStorageService: LocalStorageService, private messageLabelService: MessageLabelService) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.adjustTableHeight()
    }

    ngOnChanges(): void {
        this.calculateTotals()
    }

    //#endregion

    //#region public methods

    private adjustTableHeight(): void {
        setTimeout(() => {
            const x = document.getElementsByClassName('table-wrapper') as HTMLCollectionOf<HTMLInputElement>
            for (let i = 0; i < x.length; i++) {
                x[i].style.height = document.getElementById('content').offsetHeight - 150 + 'px'
            }
        }, 100)
    }

    private calculateTotals(): void {
        this.totals = {
            customer: { id: 0, description: '', isActive: true },
            shipOwner: { id: 0, description: '', isActive: true },
            previousBalance: this.records.reduce((sum: number, array: { previousBalance: number }) => sum + array.previousBalance, 0),
            debit: this.records.reduce((sum: number, array: { debit: number }) => sum + array.debit, 0),
            credit: this.records.reduce((sum: number, array: { credit: number }) => sum + array.credit, 0),
            actualBalance: this.records.reduce((sum: number, array: { actualBalance: number }) => sum + array.actualBalance, 0),
        }
    }

    public formatNumberToLocale(number: number, decimals = true): string {
        return formatNumber(number, this.localStorageService.getItem('language'), decimals ? '1.2' : '1.0')
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public onFilter(event: any, column: string, matchMode: string): void {
        if (event) this.table.filter(event, column, matchMode)
    }

    //#endregion

}
