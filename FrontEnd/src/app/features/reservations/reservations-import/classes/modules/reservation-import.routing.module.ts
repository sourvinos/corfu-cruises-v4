import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
// Custom
import { AuthGuardService } from 'src/app/shared/services/auth-guard.service'
import { ReservationImportListComponent } from '../../user-interface/import-list/reservation-import-list.component'

const routes: Routes = [
    { path: '', component: ReservationImportListComponent, canActivate: [AuthGuardService] }
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class ReservationImportRoutingModule { }
