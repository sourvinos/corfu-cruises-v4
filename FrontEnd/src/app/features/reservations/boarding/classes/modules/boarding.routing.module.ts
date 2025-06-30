import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
// Custom
import { AuthGuardService } from 'src/app/shared/services/auth-guard.service'
import { BoardingCriteriaComponent } from '../../user-interface/criteria/boarding-criteria.component'
import { BoardingListResolver } from '../resolvers/boarding-list.resolver'
import { BoardingReservationsComponent } from '../../user-interface/list/reservations/boarding-reservations.component'

const routes: Routes = [
    { path: '', component: BoardingCriteriaComponent, canActivate: [AuthGuardService] },
    { path: 'list', component: BoardingReservationsComponent, canActivate: [AuthGuardService], resolve: { boardingList: BoardingListResolver }, runGuardsAndResolvers: 'always' }
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class BoardingRoutingModule { }
