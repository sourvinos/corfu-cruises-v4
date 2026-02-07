import { Component, Input } from '@angular/core'
// Custom
import { HelperService } from 'src/app/shared/services/helper.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { StatisticNationalitiesVM } from '../../classes/view-models/list/statistics-nationalities-vm'
import { StatisticVM } from '../../classes/view-models/list/statistics-vm'
import { environment } from './../../../../../../environments/environment'

@Component({
    selector: 'table',
    templateUrl: './table.component.html',
    styleUrls: ['./table.component.css']
})

export class TableComponent {

    //#region variables

    @Input() array: StatisticVM[] | StatisticNationalitiesVM[]
    public feature = 'statistics'
    public totals: StatisticVM

    //#endregion

    constructor(private helperService: HelperService, private messageLabelService: MessageLabelService) { }

    //#region lifecycle hooks

    ngOnChanges(): void {
        this.getTotals()
        this.removeTotalsFromArray()
        this.calculatePercentagePerRow()
        this.calculateNoShowPerRow()
        this.calculateTotalsPercentage()
        this.calculateTotalsNoShow()
    }

    //#endregion

    //#region public methods

    public calculateNoShowPerRow(): void {
        if (this.array) {
            this.array.forEach(element => {
                element.noShow = element.pax - element.actualPax
            })
        }
    }

    public calculatePercentagePerRow(): void {
        if (this.array) {
            this.array.forEach(element => {
                element.percentage = (100 * element.actualPax / this.totals.actualPax).toFixed(2)
            })
        }
    }

    public calculateTotalsNoShow(): void {
        this.totals ? this.totals.noShow = this.totals.pax - this.totals.actualPax : 0
    }

    public calculateTotalsPercentage(): void {
        this.totals ? this.totals.percentage = (100 * this.totals.actualPax / this.totals.actualPax).toFixed(2) : '0'
    }

    public formatNumberToLocale(value: number): any {
        return value ? this.helperService.formatNumberToLocale(value) : 0
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public getNationalityIcon(record: any): any {
        return record.code ? environment.nationalitiesIconDirectory + record.code.toLowerCase() + '.png' : ''
    }

    public getTableHeight(): string {
        return document.getElementById('content').clientHeight - 150 + 'px'
    }

    public isNationality(record: any): boolean {
        return record.code
    }

    //#endregion

    //#region private methods

    private getTotals(): void {
        this.totals = this.array ? this.array[this.array.length - 1] : undefined
    }

    private removeTotalsFromArray(): void {
        this.totals ? this.array.pop() : undefined
    }

    //#endregion

}
