import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
// Custom
import { AuthGuardService } from 'src/app/shared/services/auth-guard.service'
import { InvoiceFormComponent } from '../../user-interface/invoice-form/invoice-form.component'
import { InvoiceFormResolver } from '../resolvers/invoice-form.resolver'
import { InvoiceListComponent } from '../../user-interface/invoice-list/invoice-list.component'
import { InvoiceListResolver } from '../resolvers/invoice-list.resolver'

const routes: Routes = [
    { path: '', component: InvoiceListComponent, canActivate: [AuthGuardService], resolve: { invoiceList: InvoiceListResolver }, runGuardsAndResolvers: 'always' },
    { path: 'new', component: InvoiceFormComponent, canActivate: [AuthGuardService] },
    { path: ':id', component: InvoiceFormComponent, canActivate: [AuthGuardService], resolve: { invoiceForm: InvoiceFormResolver } },
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class InvoiceRoutingModule { }
