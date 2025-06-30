import { NgModule } from '@angular/core'
// Custom
import { SharedModule } from '../../../../../shared/modules/shared.module'
import { TaxOfficeFormComponent } from '../../user-interface/taxOffice-form.component'
import { TaxOfficeListComponent } from '../../user-interface/taxOffice-list.component'
import { TaxOfficeRoutingModule } from './taxOffice.routing.module'

@NgModule({
    declarations: [
        TaxOfficeListComponent,
        TaxOfficeFormComponent
    ],
    imports: [
        SharedModule,
        TaxOfficeRoutingModule
    ]
})

export class TaxOfficeModule { }
