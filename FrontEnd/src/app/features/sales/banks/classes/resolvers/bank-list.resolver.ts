import { Injectable } from '@angular/core'
import { Observable, of } from 'rxjs'
import { catchError, map } from 'rxjs/operators'
// Custom
import { BankService } from '../services/bank.service'
import { ListResolved } from '../../../../../shared/classes/list-resolved'

@Injectable({ providedIn: 'root' })

export class BankListResolver {

    constructor(private bankService: BankService) { }

    resolve(): Observable<ListResolved> {
        return this.bankService.getAll().pipe(
            map((bankList) => new ListResolved(bankList)),
            catchError((err: any) => of(new ListResolved(null, err)))
        )
    }

}
