import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
// Custom
import { AuthGuardService } from 'src/app/shared/services/auth-guard.service'
import { LedgerParentSalesComponent } from '../../user-interface/list/ledger-parent.component'

const routes: Routes = [
    { path: '', component: LedgerParentSalesComponent, canActivate: [AuthGuardService], runGuardsAndResolvers: 'always' }
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class LedgerSalesRoutingModule { }
