import { NgModule } from '@angular/core'
// Custom
import { LedgerSalesRoutingModule } from './ledger-sales.routing.module'
import { LedgerCriteriaDialogComponent } from '../../user-interface/criteria/ledger-criteria.component'
import { LedgerParentSalesComponent } from '../../user-interface/list/ledger-parent.component'
import { LedgerShipOwnerTableComponent } from '../../user-interface/list/ledger-shipOwner-table.component'
import { SharedModule } from '../../../../../shared/modules/shared.module'

@NgModule({
    declarations: [
        LedgerCriteriaDialogComponent,
        LedgerParentSalesComponent,
        LedgerShipOwnerTableComponent
    ],
    imports: [
        SharedModule,
        LedgerSalesRoutingModule
    ]
})

export class LedgerSalesModule { }
