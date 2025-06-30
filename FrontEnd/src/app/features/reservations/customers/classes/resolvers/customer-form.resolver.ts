import { ActivatedRouteSnapshot } from '@angular/router'
import { Injectable } from '@angular/core'
import { catchError, map, of } from 'rxjs'
// Custom
import { CustomerHttpService } from '../services/customer-http.service'
import { FormResolved } from 'src/app/shared/classes/form-resolved'

@Injectable({ providedIn: 'root' })

export class CustomerFormResolver {

    constructor(private customerHttpService: CustomerHttpService) { }

    resolve(route: ActivatedRouteSnapshot): any {
        return this.customerHttpService.getSingle(route.params.id).pipe(
            map((customerForm) => new FormResolved(customerForm)),
            catchError((err: any) => of(new FormResolved(null, err)))
        )
    }

}
