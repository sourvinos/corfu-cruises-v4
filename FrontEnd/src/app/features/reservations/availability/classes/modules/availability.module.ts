import { NgModule } from '@angular/core'
// Custom
import { AvailabilityComponent } from '../../user-interface/calendar/availability.component'
import { AvailabilityRoutingModule } from './availability.routing.module'
import { ModalDebugDialogComponent } from '../../user-interface/modal-debug-dialog/modal-debug-dialog.component'
import { SharedModule } from '../../../../../shared/modules/shared.module'

@NgModule({
    declarations: [
        AvailabilityComponent,
        ModalDebugDialogComponent
    ],
    imports: [
        SharedModule,
        AvailabilityRoutingModule
    ]
})

export class AvailabilityModule { }
