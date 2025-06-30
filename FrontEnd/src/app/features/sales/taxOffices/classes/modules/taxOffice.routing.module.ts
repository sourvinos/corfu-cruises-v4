import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
// Custom
import { AuthGuardService } from 'src/app/shared/services/auth-guard.service'
import { TaxOfficeFormComponent } from '../../user-interface/taxOffice-form.component'
import { TaxOfficeFormResolver } from '../resolvers/taxOffice-form.resolver'
import { TaxOfficeListComponent } from '../../user-interface/taxOffice-list.component'
import { TaxOfficeListResolver } from '../resolvers/taxOffice-list.resolver'

const routes: Routes = [
    { path: '', component: TaxOfficeListComponent, canActivate: [AuthGuardService], resolve: { taxOfficeList: TaxOfficeListResolver } },
    { path: 'new', component: TaxOfficeFormComponent, canActivate: [AuthGuardService] },
    { path: ':id', component: TaxOfficeFormComponent, canActivate: [AuthGuardService], resolve: { taxOfficeForm: TaxOfficeFormResolver } }
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class TaxOfficeRoutingModule { }
