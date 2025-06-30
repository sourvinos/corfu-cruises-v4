import { Injectable } from '@angular/core'
import { Observable, of } from 'rxjs'
import { catchError, map } from 'rxjs/operators'
// Custom
import { DocumentTypeHttpService } from '../services/documentType-http.service'
import { ListResolved } from '../../../../../shared/classes/list-resolved'

@Injectable({ providedIn: 'root' })

export class DocumentTypeListResolver {

    constructor(private documentTypeHttpService: DocumentTypeHttpService) { }

    resolve(): Observable<ListResolved> {
        return this.documentTypeHttpService.getAll().pipe(
            map((documentTypeList) => new ListResolved(documentTypeList)),
            catchError((err: any) => of(new ListResolved(null, err)))
        )
    }

}
