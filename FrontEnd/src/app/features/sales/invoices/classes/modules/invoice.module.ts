import { NgModule } from '@angular/core'
// Custom
import { AadePanelComponent } from '../../user-interface/aade-panel/aade-panel.component'
import { InvoiceFormComponent } from '../../user-interface/invoice-form/invoice-form.component'
import { InvoiceListComponent } from '../../user-interface/invoice-list/invoice-list.component'
import { InvoiceRoutingModule } from './invoice.routing.module'
import { SharedModule } from '../../../../../shared/modules/shared.module'

@NgModule({
    declarations: [
        AadePanelComponent,
        InvoiceFormComponent,
        InvoiceListComponent,
    ],
    imports: [
        SharedModule,
        InvoiceRoutingModule
    ]
})

export class InvoiceModule { }
