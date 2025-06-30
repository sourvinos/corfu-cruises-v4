import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
// Custom
import { AuthGuardService } from 'src/app/shared/services/auth-guard.service'
import { BalanceSheetParentComponent } from '../../user-interface/balanceSheet-parent.component'

const routes: Routes = [
    { path: '', component: BalanceSheetParentComponent, canActivate: [AuthGuardService], runGuardsAndResolvers: 'always' }
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class BalanceSheetRoutingModule { }
