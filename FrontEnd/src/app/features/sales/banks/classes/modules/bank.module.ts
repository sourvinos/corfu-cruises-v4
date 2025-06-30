import { NgModule } from '@angular/core'
// Custom
import { BankFormComponent } from '../../user-interface/bank-form.component'
import { BankListComponent } from '../../user-interface/bank-list.component'
import { BankRoutingModule } from './bank.routing.module'
import { SharedModule } from '../../../../../shared/modules/shared.module'

@NgModule({
    declarations: [
        BankListComponent,
        BankFormComponent
    ],
    imports: [
        SharedModule,
        BankRoutingModule
    ]
})

export class BankModule { }
