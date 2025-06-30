import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
// Custom
import { AuthGuardService } from 'src/app/shared/services/auth-guard.service'
import { DocumentTypeFormComponent } from '../../user-interface/documentType-form.component'
import { DocumentTypeFormResolver } from '../resolvers/documentType-form.resolver'
import { DocumentTypeListComponent } from '../../user-interface/documentType-list.component'
import { DocumentTypeListResolver } from '../resolvers/documentType-list.resolver'

const routes: Routes = [
    { path: '', component: DocumentTypeListComponent, canActivate: [AuthGuardService], resolve: { documentTypeList: DocumentTypeListResolver } },
    { path: 'new', component: DocumentTypeFormComponent, canActivate: [AuthGuardService] },
    { path: ':id', component: DocumentTypeFormComponent, canActivate: [AuthGuardService], resolve: { documentTypeForm: DocumentTypeFormResolver } }
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class DocumentTypeRoutingModule { }
