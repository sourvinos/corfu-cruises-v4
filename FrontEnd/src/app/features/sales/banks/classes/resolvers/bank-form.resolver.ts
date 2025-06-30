import { ActivatedRouteSnapshot } from '@angular/router'
import { Injectable } from '@angular/core'
import { catchError, map, of } from 'rxjs'
// Custom
import { BankService } from '../services/bank.service'
import { FormResolved } from 'src/app/shared/classes/form-resolved'

@Injectable({ providedIn: 'root' })

export class BankFormResolver {

    constructor(private bankService: BankService) { }

    resolve(route: ActivatedRouteSnapshot): any {
        return this.bankService.getSingle(route.params.id).pipe(
            map((bankForm) => new FormResolved(bankForm)),
            catchError((err: any) => of(new FormResolved(null, err)))
        )
    }

}
