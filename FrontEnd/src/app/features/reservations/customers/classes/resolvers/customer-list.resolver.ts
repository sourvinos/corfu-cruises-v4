import { Injectable } from '@angular/core'
import { Observable, of } from 'rxjs'
import { catchError, map } from 'rxjs/operators'
// Custom
import { CustomerHttpService } from '../services/customer-http.service'
import { ListResolved } from '../../../../../shared/classes/list-resolved'

@Injectable({ providedIn: 'root' })

export class CustomerListResolver {

    constructor(private customerHttpService: CustomerHttpService) { }

    resolve(): Observable<ListResolved> {
        return this.customerHttpService.getAll().pipe(
            map((customerList) => new ListResolved(customerList)),
            catchError((err: any) => of(new ListResolved(null, err)))
        )
    }

}
