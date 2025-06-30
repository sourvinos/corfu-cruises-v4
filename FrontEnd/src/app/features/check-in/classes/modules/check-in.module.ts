import { NgModule } from '@angular/core'
// Components
import { CheckInRoutingModule } from './check-in.routing.module'
import { CompletionComponent } from './../../user-interface/7-completion/completion.component'
import { EmailFormComponent } from '../../user-interface/6-email/email-form.component'
import { GreetingComponent } from '../../user-interface/1-greeting/greeting.component'
import { HelpComponent } from '../../user-interface/8-help/help.component'
import { PassengerFormComponent } from '../../user-interface/5-passenger-form/passenger-form.component'
import { PassengerListComponent } from '../../user-interface/4-passenger-list/passenger-list.component'
import { ReservationComponent } from '../../user-interface/3-reservation/reservation.component'
import { SearchComponent } from '../../user-interface/2-search/search.component'
import { SharedModule } from 'src/app/shared/modules/shared.module'

@NgModule({
    declarations: [
        GreetingComponent,
        SearchComponent,
        ReservationComponent,
        PassengerListComponent,
        PassengerFormComponent,
        EmailFormComponent,
        CompletionComponent,
        HelpComponent
    ],
    imports: [
        SharedModule,
        CheckInRoutingModule
    ],
})

export class CheckInModule { }
