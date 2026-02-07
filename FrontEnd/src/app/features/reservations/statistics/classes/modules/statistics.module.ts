import { NgModule } from '@angular/core'
// Custom
import { SharedModule } from 'src/app/shared/modules/shared.module'
import { StatisticsComponent } from '../../user-interface/list/statistics.component'
import { StatisticsCriteriaDialogComponent } from '../../user-interface/criteria/statistics-criteria-dialog.component'
import { StatisticsRoutingModule } from './statistics.routing.module'
import { TableComponent } from '../../user-interface/list/table.component'

@NgModule({
    declarations: [
        StatisticsCriteriaDialogComponent,
        StatisticsComponent,
        TableComponent
    ],
    imports: [
        SharedModule,
        StatisticsRoutingModule,
    ]
})

export class StatisticsModule { }
