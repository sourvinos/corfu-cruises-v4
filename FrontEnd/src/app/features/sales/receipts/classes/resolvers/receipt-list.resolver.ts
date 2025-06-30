import { Injectable } from '@angular/core'
import { Observable, of } from 'rxjs'
import { catchError, map } from 'rxjs/operators'
// Custom
import { ListResolved } from '../../../../../shared/classes/list-resolved'
import { ReceiptHttpService } from '../services/receipt-http.service'

@Injectable({ providedIn: 'root' })

export class ReceiptListResolver {

    constructor(private receiptHttpService: ReceiptHttpService) { }

    resolve(): Observable<ListResolved> {
        return this.receiptHttpService.getAll()
            .pipe(
                map((invoiceList) => new ListResolved(invoiceList)),
                catchError((err: any) => of(new ListResolved(null, err)))
            )
    }

}
