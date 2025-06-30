import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
// Custom
import { AuthGuardService } from 'src/app/shared/services/auth-guard.service'
import { PortFormComponent } from '../../user-interface/port-form.component'
import { PortFormResolver } from '../resolvers/port-form.resolver'
import { PortListComponent } from '../../user-interface/port-list.component'
import { PortListResolver } from '../resolvers/port-list.resolver'

const routes: Routes = [
    { path: '', component: PortListComponent, canActivate: [AuthGuardService], resolve: { portList: PortListResolver } },
    { path: 'new', component: PortFormComponent, canActivate: [AuthGuardService] },
    { path: ':id', component: PortFormComponent, canActivate: [AuthGuardService], resolve: { portForm: PortFormResolver } }
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class PortRoutingModule { }