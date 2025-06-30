import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
// Custom
import { AuthGuardService } from 'src/app/shared/services/auth-guard.service'
import { BankFormComponent } from '../../user-interface/bank-form.component'
import { BankFormResolver } from '../resolvers/bank-form.resolver'
import { BankListComponent } from '../../user-interface/bank-list.component'
import { BankListResolver } from '../resolvers/bank-list.resolver'

const routes: Routes = [
    { path: '', component: BankListComponent, canActivate: [AuthGuardService], resolve: { bankList: BankListResolver } },
    { path: 'new', component: BankFormComponent, canActivate: [AuthGuardService] },
    { path: ':id', component: BankFormComponent, canActivate: [AuthGuardService], resolve: { bankForm: BankFormResolver } }
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class BankRoutingModule { }
