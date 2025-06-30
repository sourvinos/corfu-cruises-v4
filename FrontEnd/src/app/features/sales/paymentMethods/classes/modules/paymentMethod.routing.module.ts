import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
// Custom
import { AuthGuardService } from 'src/app/shared/services/auth-guard.service'
import { PaymentMethodFormComponent } from '../../user-interface/paymentMethod-form.component'
import { PaymentMethodFormResolver } from '../resolvers/paymentMethod-form.resolver'
import { PaymentMethodListComponent } from '../../user-interface/paymentMethod-list.component'
import { PaymentMethodListResolver } from '../resolvers/paymentMethod-list.resolver'

const routes: Routes = [
    { path: '', component: PaymentMethodListComponent, canActivate: [AuthGuardService], resolve: { paymentMethodList: PaymentMethodListResolver } },
    { path: 'new', component: PaymentMethodFormComponent, canActivate: [AuthGuardService] },
    { path: ':id', component: PaymentMethodFormComponent, canActivate: [AuthGuardService], resolve: { paymentMethodForm: PaymentMethodFormResolver } }
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class PaymentMethodRoutingModule { }
