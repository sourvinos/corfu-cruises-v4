import { ActivatedRouteSnapshot } from '@angular/router'
import { Injectable } from '@angular/core'
import { catchError, map, of } from 'rxjs'
// Custom
import { FormResolved } from 'src/app/shared/classes/form-resolved'
import { PaymentMethodHttpService } from '../services/paymentMethod-http.service'

@Injectable({ providedIn: 'root' })

export class PaymentMethodFormResolver {

    constructor(private paymentMethodHttpService: PaymentMethodHttpService) { }

    resolve(route: ActivatedRouteSnapshot): any {
        return this.paymentMethodHttpService.getSingle(route.params.id).pipe(
            map((paymentMethodForm) => new FormResolved(paymentMethodForm)),
            catchError((err: any) => of(new FormResolved(null, err)))
        )
    }

}
