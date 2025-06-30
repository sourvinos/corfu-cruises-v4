import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
// Custom
import { AuthGuardService } from 'src/app/shared/services/auth-guard.service'
import { ReceiptFormComponent } from '../../user-interface/form/receipt-form.component'
import { ReceiptFormResolver } from '../resolvers/receipt-form.resolver'
import { ReceiptListComponent } from '../../user-interface/list/receipt-list.component'
import { ReceiptListResolver } from '../resolvers/receipt-list.resolver'

const routes: Routes = [
    { path: '', component: ReceiptListComponent, canActivate: [AuthGuardService], resolve: { receiptList: ReceiptListResolver }, runGuardsAndResolvers: 'always' },
    { path: 'new', component: ReceiptFormComponent, canActivate: [AuthGuardService] },
    { path: ':id', component: ReceiptFormComponent, canActivate: [AuthGuardService], resolve: { receiptForm: ReceiptFormResolver } },
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class ReceiptRoutingModule { }
