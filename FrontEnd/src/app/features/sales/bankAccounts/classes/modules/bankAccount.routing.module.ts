import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
// Custom
import { AuthGuardService } from 'src/app/shared/services/auth-guard.service'
import { BankAccountFormComponent } from '../../user-interface/bankAccount-form.component'
import { BankAccountFormResolver } from '../resolvers/bankAccount-form.resolver'
import { BankAccountListComponent } from '../../user-interface/bankAccount-list.component'
import { BankAccountListResolver } from '../resolvers/bankAccount-list.resolver'

const routes: Routes = [
    { path: '', component: BankAccountListComponent, canActivate: [AuthGuardService], resolve: { bankAccountList: BankAccountListResolver } },
    { path: 'new', component: BankAccountFormComponent, canActivate: [AuthGuardService] },
    { path: ':id', component: BankAccountFormComponent, canActivate: [AuthGuardService], resolve: { bankAccountForm: BankAccountFormResolver } }
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class BankAccountRoutingModule { }