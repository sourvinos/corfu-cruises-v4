import { NgModule } from '@angular/core'
// Custom
import { DocumentTypeFormComponent } from '../../user-interface/documentType-form.component'
import { DocumentTypeListComponent } from '../../user-interface/documentType-list.component'
import { DocumentTypeRoutingModule } from './documentType.routing.module'
import { SharedModule } from '../../../../../shared/modules/shared.module'

@NgModule({
    declarations: [
        DocumentTypeListComponent,
        DocumentTypeFormComponent
    ],
    imports: [
        DocumentTypeRoutingModule,
        SharedModule
    ]
})

export class DocumentTypeModule { }
