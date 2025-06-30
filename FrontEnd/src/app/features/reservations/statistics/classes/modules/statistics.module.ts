import { NgModule } from '@angular/core'
// Custom
import { SharedModule } from 'src/app/shared/modules/shared.module'
import { StatisticsComponent } from '../../user-interface/statistics.component'
import { StatisticsRoutingModule } from './statistics.routing.module'
import { TableComponent } from '../../user-interface/table.component'
import { TableNationalitiesComponent } from '../../user-interface/table-nationalities.component'

@NgModule({
    declarations: [
        StatisticsComponent,
        TableComponent,
        TableNationalitiesComponent
    ],
    imports: [
        SharedModule,
        StatisticsRoutingModule,
    ]
})

export class StatisticsModule { }
