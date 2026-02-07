import { Component } from '@angular/core'
import { MatDialog } from '@angular/material/dialog'
import { Router } from '@angular/router'
// Custom
import { DateHelperService } from 'src/app/shared/services/date-helper.service'
import { HelperService } from 'src/app/shared/services/helper.service'
import { InteractionService } from 'src/app/shared/services/interaction.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { StatisticNationalitiesVM } from '../../classes/view-models/list/statistics-nationalities-vm'
import { StatisticVM } from '../../classes/view-models/list/statistics-vm'
import { StatisticsCriteriaDialogComponent } from './../criteria/statistics-criteria-dialog.component'
import { StatisticsCriteriaVM } from '../../classes/view-models/criteria/statistics-criteria-vm'
import { StatisticsHttpService } from '../../classes/services/statistics-http.service'

@Component({
    selector: 'statistics',
    templateUrl: './statistics.component.html',
    styleUrls: ['../../../../../../assets/styles/custom/lists.css', './statistics.component.css']
})

export class StatisticsComponent {

    //#region variables

    public ytd: StatisticVM[]
    public customers: StatisticVM[]
    public destinations: StatisticVM[]
    public drivers: StatisticVM[]
    public nationalities: StatisticNationalitiesVM[]
    public ports: StatisticVM[]
    public ships: StatisticVM[]
    public feature = 'statistics'
    public featureIcon = 'statistics'
    public icon = 'arrow_back'
    public parentUrl = '/home'
    private criteria: StatisticsCriteriaVM

    //#endregion

    constructor(private dateHelperService: DateHelperService, private helperService: HelperService, private interactionService: InteractionService, private messageLabelService: MessageLabelService, private router: Router, private statisticsService: StatisticsHttpService, public dialog: MatDialog) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.setTabTitle()
        this.subscribeToInteractionService()
        this.onSelectedTabChange()
    }

    //#endregion

    //#region public methods

    public getCriteria(): string {
        if (this.criteria) {
            return this.criteria.fromDate + ' - ' + this.criteria.toDate
        }
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public onClose(): void {
        this.goBack()
    }

    public onShowCriteriaDialog(): void {
        const dialogRef = this.dialog.open(StatisticsCriteriaDialogComponent, {
            data: ['statistics-criteria'],
            height: '36.0625rem',
            panelClass: 'dialog',
            width: '32rem',
        })
        dialogRef.afterClosed().subscribe(criteria => {
            if (criteria !== undefined) {
                this.buildCriteriaVM(criteria)
                this.statisticsService.getStatistics('ytd', criteria).subscribe(response => { this.ytd = [...response] })
                this.statisticsService.getStatistics('customers', criteria).subscribe(response => { this.customers = [...response] })
                this.statisticsService.getStatistics('destinations', criteria).subscribe(response => { this.destinations = [...response] })
                this.statisticsService.getStatistics('drivers', criteria).subscribe(response => { this.drivers = [...response] })
                this.statisticsService.getStatistics('nationalities', criteria).subscribe(response => { this.nationalities = [...response] })
                this.statisticsService.getStatistics('ports', criteria).subscribe(response => { this.ports = [...response] })
                this.statisticsService.getStatistics('ships', criteria).subscribe(response => { this.ships = [...response] })
            }
        })
    }

    public onSelectedTabChange(): void {
        setTimeout(() => {
            const x = document.getElementsByClassName('table-wrapper') as HTMLCollectionOf<HTMLInputElement>
            for (let i = 0; i < x.length; i++) {
                x[i].style.height = document.getElementById('content').offsetHeight - 150 + 'px'
            }
        }, 100)
    }

    //#endregion

    //#region private methods

    private buildCriteriaVM(criteria: StatisticsCriteriaVM): StatisticsCriteriaVM {
        const x: StatisticsCriteriaVM = {
            fromDate: criteria.fromDate,
            toDate: criteria.toDate
        }
        return x
    }

    private goBack(): void {
        this.router.navigate([this.parentUrl])
    }

    private setTabTitle(): void {
        this.helperService.setTabTitle(this.feature)
    }

    private subscribeToInteractionService(): void {
        this.interactionService.refreshTabTitle.subscribe(() => { this.setTabTitle() })
        this.interactionService.emitDateRange.subscribe((response) => {
            if (response) {
                this.criteria = {
                    fromDate: this.dateHelperService.formatISODateToLocale(response.fromDate),
                    toDate: this.dateHelperService.formatISODateToLocale(response.toDate)
                }
            }
        })
    }

    //#endregion

}
