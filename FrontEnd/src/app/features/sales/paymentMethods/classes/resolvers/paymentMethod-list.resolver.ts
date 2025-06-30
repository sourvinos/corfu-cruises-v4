import { Injectable } from '@angular/core'
import { Observable, of } from 'rxjs'
import { catchError, map } from 'rxjs/operators'
// Custom
import { ListResolved } from '../../../../../shared/classes/list-resolved'
import { PaymentMethodHttpService } from '../services/paymentMethod-http.service'

@Injectable({ providedIn: 'root' })

export class PaymentMethodListResolver {

    constructor(private paymentMethodHttpService: PaymentMethodHttpService) { }

    resolve(): Observable<ListResolved> {
        return this.paymentMethodHttpService.getAll().pipe(
            map((paymentMethodList) => new ListResolved(paymentMethodList)),
            catchError((err: any) => of(new ListResolved(null, err)))
        )
    }

}
