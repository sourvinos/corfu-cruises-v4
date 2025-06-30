import { ActivatedRouteSnapshot } from '@angular/router'
import { Injectable } from '@angular/core'
import { catchError, map, of } from 'rxjs'
// Custom
import { FormResolved } from 'src/app/shared/classes/form-resolved'
import { InvoiceHttpDataService } from '../services/invoice-http-data.service'

@Injectable({ providedIn: 'root' })

export class InvoiceFormResolver {

    constructor(private invoiceHttpService: InvoiceHttpDataService) { }

    resolve(route: ActivatedRouteSnapshot): any {
        return this.invoiceHttpService.getSingle(route.params.id).pipe(
            map((invoiceForm) => new FormResolved(invoiceForm)),
            catchError((err: any) => of(new FormResolved(null, err)))
        )
    }

}
