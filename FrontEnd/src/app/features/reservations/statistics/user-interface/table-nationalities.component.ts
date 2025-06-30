import { Component, Input } from '@angular/core'
// Custom
import { HelperService } from 'src/app/shared/services/helper.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { StatisticsNationalitiesVM } from '../classes/view-models/statistics-nationalities-vm'
import { environment } from 'src/environments/environment'

@Component({
    selector: 'table-nationalities',
    templateUrl: './table-nationalities.component.html',
    styleUrls: ['./table-nationalities.component.css']
})

export class TableNationalitiesComponent {

    //#region variables

    @Input() array: StatisticsNationalitiesVM[]

    public feature = 'statistics'
    public totals: StatisticsNationalitiesVM

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

    public getNationalityIcon(nationalityCode: string): any {
        if (nationalityCode != undefined) {
            return environment.nationalitiesIconDirectory + nationalityCode.toLowerCase() + '.png'
        }
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
