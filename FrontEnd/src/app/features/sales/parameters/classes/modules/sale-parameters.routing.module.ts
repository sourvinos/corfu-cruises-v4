import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
// Custom
import { AuthGuardService } from 'src/app/shared/services/auth-guard.service'
import { SaleParametersComponent } from '../../user-interface/sale-parameters.component'
import { SaleParametersResolver } from '../resolvers/sale-parameters.resolver'

const routes: Routes = [
    { path: '', component: SaleParametersComponent, canActivate: [AuthGuardService], resolve: { parameters: SaleParametersResolver } }
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class SaleParametersRoutingModule { }
