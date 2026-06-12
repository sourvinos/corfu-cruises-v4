import { NgModule } from '@angular/core'
// Custom
import { SaleParametersComponent } from '../../user-interface/sale-parameters.component'
import { SaleParametersRoutingModule } from './sale-parameters.routing.module'
import { SharedModule } from '../../../../../shared/modules/shared.module'

@NgModule({
    declarations: [
        SaleParametersComponent
    ],
    imports: [
        SaleParametersRoutingModule,
        SharedModule,
    ]
})

export class SaleParametersModule { }
