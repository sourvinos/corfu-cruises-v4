import { ActivatedRouteSnapshot } from '@angular/router'
import { Injectable } from '@angular/core'
import { catchError, map, of } from 'rxjs'
// Custom
import { BankAccountHttpService } from '../services/bankAccount-http.service'
import { FormResolved } from 'src/app/shared/classes/form-resolved'

@Injectable({ providedIn: 'root' })

export class BankAccountFormResolver {

    constructor(private bankAccountHttpService: BankAccountHttpService) { }

    resolve(route: ActivatedRouteSnapshot): any {
        return this.bankAccountHttpService.getSingle(route.params.id).pipe(
            map((form) => new FormResolved(form)),
            catchError((err: any) => of(new FormResolved(null, err)))
        )
    }

}
