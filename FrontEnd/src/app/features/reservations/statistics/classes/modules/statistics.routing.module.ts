import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
// Custom
import { AuthGuardService } from 'src/app/shared/services/auth-guard.service'
import { StatisticsComponent } from '../../user-interface/list/statistics.component'

const routes: Routes = [
    { path: '', component: StatisticsComponent, canActivate: [AuthGuardService], runGuardsAndResolvers: 'always' }
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class StatisticsRoutingModule { }
