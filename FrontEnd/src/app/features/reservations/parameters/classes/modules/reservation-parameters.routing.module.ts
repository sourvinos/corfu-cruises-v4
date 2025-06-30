import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
// Custom
import { AuthGuardService } from 'src/app/shared/services/auth-guard.service'
import { ReservationParametersComponent } from '../../user-interface/reservation-parameters.component'
import { ReservationParametersResolver } from '../resolvers/reservation-parameters.resolver'

const routes: Routes = [
    { path: '', component: ReservationParametersComponent, canActivate: [AuthGuardService], resolve: { parameters: ReservationParametersResolver } }
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class ReservationParametersRoutingModule { }
