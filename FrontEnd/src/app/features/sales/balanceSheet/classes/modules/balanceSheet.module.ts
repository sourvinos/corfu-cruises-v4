import { NgModule } from '@angular/core'
// Custom
import { BalanceSheetParentComponent } from '../../user-interface/balanceSheet-parent.component'
import { BalanceSheetRoutingModule } from './balanceSheet.routing.module'
import { BalanceSheetShipOwnerTableComponent } from '../../user-interface/balanceSheet-shipOwner-table.component'
import { SharedModule } from '../../../../../shared/modules/shared.module'

@NgModule({
    declarations: [
        BalanceSheetParentComponent,
        BalanceSheetShipOwnerTableComponent
    ],
    imports: [
        SharedModule,
        BalanceSheetRoutingModule
    ]
})

export class BalanceSheetModule { }
