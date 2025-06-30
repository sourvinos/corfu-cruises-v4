import { NgModule } from '@angular/core'
// Custom
import { ReservationParametersComponent } from '../../user-interface/reservation-parameters.component'
import { ReservationParametersRoutingModule } from './reservation-parameters.routing.module'
import { SharedModule } from '../../../../../shared/modules/shared.module'

@NgModule({
    declarations: [
        ReservationParametersComponent
    ],
    imports: [
        ReservationParametersRoutingModule,
        SharedModule,
    ]
})

export class ReservationParametersModule { }
