import { Injectable } from '@angular/core'
import { Observable, of } from 'rxjs'
import { catchError, map } from 'rxjs/operators'
// Custom
import { ListResolved } from '../../../../../shared/classes/list-resolved'
import { InvoiceHttpDataService } from '../services/invoice-http-data.service'

@Injectable({ providedIn: 'root' })

export class InvoiceListResolver {

    constructor(private invoiceHttpService: InvoiceHttpDataService) { }

    resolve(): Observable<ListResolved> {
        return this.invoiceHttpService.getAll()
            .pipe(
                map((invoiceList) => new ListResolved(invoiceList)),
                catchError((err: any) => of(new ListResolved(null, err)))
            )
    }

}
