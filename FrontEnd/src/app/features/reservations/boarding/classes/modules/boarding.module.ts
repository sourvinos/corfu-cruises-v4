import { NgModule } from '@angular/core'
// Custom
import { BoardingCriteriaComponent } from '../../user-interface/criteria/boarding-criteria.component'
import { BoardingPassengerListComponent } from '../../user-interface/list/passengers/boarding-passengers.component'
import { BoardingReservationsComponent } from '../../user-interface/list/reservations/boarding-reservations.component'
import { BoardingRoutingModule } from './boarding.routing.module'
import { SharedModule } from 'src/app/shared/modules/shared.module'

@NgModule({
    declarations: [
        BoardingCriteriaComponent,
        BoardingReservationsComponent,
        BoardingPassengerListComponent
    ],
    imports: [
        SharedModule,
        BoardingRoutingModule
    ]
})

export class BoardingModule { }
