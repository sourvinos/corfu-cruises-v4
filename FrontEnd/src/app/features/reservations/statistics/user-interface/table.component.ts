import { Component, Input } from '@angular/core'
// Custom
import { HelperService } from 'src/app/shared/services/helper.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { StatisticsVM } from '../classes/view-models/statistics-vm'

@Component({
    selector: 'table',
    templateUrl: './table.component.html',
    styleUrls: ['./table.component.css']
})

export class TableComponent {

    //#region variables

    @Input() array: StatisticsVM[]

    public feature = 'statistics'
    public totals: StatisticsVM

    //#endregion

    constructor(private helperService: HelperService, private messageLabelService: MessageLabelService) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.getTotals()
        this.removeTotalsFromArray()
        this.calculatePercentagePerRow()
        this.calculateNoShowPerRow()
        this.calculateTotalsPercentage()
        this.calculateTotalsNoShow()
    }

    //#endregion

    //#region public methods

    public calculatePercentagePerRow(): void {
        this.array.forEach(element => {
            element.percentage = (100 * element.actualPax / this.totals.actualPax).toFixed(2)
        })
    }

    public calculateNoShowPerRow(): void {
        this.array.forEach(element => {
            element.noShow = element.pax - element.actualPax
        })
    }

    public calculateTotalsPercentage(): void {
        this.totals.percentage = (100 * this.totals.actualPax / this.totals.actualPax).toFixed(2)
    }

    public calculateTotalsNoShow(): void {
        this.totals.noShow = this.totals.pax - this.totals.actualPax
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public getTableHeight(): string {
        return document.getElementById('content').clientHeight - 150 + 'px'
    }

    public formatNumberToLocale(value: number): any {
        return this.helperService.formatNumberToLocale(value)
    }

    //#endregion

    //#region private methods

    private getTotals(): void {
        this.totals = this.array[this.array.length - 1]
    }

    private removeTotalsFromArray(): void {
        this.array.pop()
    }

    //#endregion

}
