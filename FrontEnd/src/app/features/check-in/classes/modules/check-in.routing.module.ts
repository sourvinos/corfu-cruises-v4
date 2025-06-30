import { NgModule } from '@angular/core'
import { RouterModule, Routes } from '@angular/router'
// Custom
import { CompletionComponent } from '../../user-interface/7-completion/completion.component'
import { EmailFormComponent } from '../../user-interface/6-email/email-form.component'
import { GreetingComponent } from '../../user-interface/1-greeting/greeting.component'
import { PassengerListComponent } from '../../user-interface/4-passenger-list/passenger-list.component'
import { ReservationComponent } from '../../user-interface/3-reservation/reservation.component'
import { SearchComponent } from '../../user-interface/2-search/search.component'

const routes: Routes = [
    { path: '', component: GreetingComponent },
    { path: 'search', component: SearchComponent },
    { path: 'reservation', component: ReservationComponent },
    { path: 'passenger-list', component: PassengerListComponent },
    { path: 'email', component: EmailFormComponent },
    { path: 'completion', component: CompletionComponent },
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class CheckInRoutingModule { }
