import { NgModule } from '@angular/core'
// Custom
import { ReservationImportListComponent } from '../../user-interface/invoice-list/reservation-import-list.component'
import { ReservationImportRoutingModule } from './reservation-import.routing.module'
import { SharedModule } from '../../../../../shared/modules/shared.module'

@NgModule({
    declarations: [
        ReservationImportListComponent
    ],
    imports: [
        SharedModule,
        ReservationImportRoutingModule
    ]
})

export class ReservationImportModule { }
