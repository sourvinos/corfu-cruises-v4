import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
// Custom
import { AuthGuardService } from 'src/app/shared/services/auth-guard.service'
import { PriceFormComponent } from '../../user-interface/price-form/price-form.component'
import { PriceFormResolver } from '../resolvers/price-form.resolver'
import { PriceListComponent } from '../../user-interface/price-list/price-list.component'
import { PriceListResolver } from '../resolvers/price-list.resolver'

const routes: Routes = [
    { path: '', component: PriceListComponent, canActivate: [AuthGuardService], resolve: { priceList: PriceListResolver }, runGuardsAndResolvers: 'always' },
    { path: 'new', component: PriceFormComponent, canActivate: [AuthGuardService] },
    { path: ':id', component: PriceFormComponent, canActivate: [AuthGuardService], resolve: { priceForm: PriceFormResolver } },
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class PriceRoutingModule { }
