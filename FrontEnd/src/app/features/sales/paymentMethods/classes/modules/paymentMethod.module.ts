import { NgModule } from '@angular/core'
// Custom
import { PaymentMethodFormComponent } from '../../user-interface/paymentMethod-form.component'
import { PaymentMethodListComponent } from '../../user-interface/paymentMethod-list.component'
import { PaymentMethodRoutingModule } from './paymentMethod.routing.module'
import { SharedModule } from '../../../../../shared/modules/shared.module'

@NgModule({
    declarations: [
        PaymentMethodListComponent,
        PaymentMethodFormComponent
    ],
    imports: [
        PaymentMethodRoutingModule,
        SharedModule
    ]
})

export class PaymentMethodModule { }
