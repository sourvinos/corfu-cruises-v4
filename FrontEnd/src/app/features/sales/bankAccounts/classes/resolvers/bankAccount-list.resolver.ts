import { Injectable } from '@angular/core'
import { Observable, of } from 'rxjs'
import { catchError, map } from 'rxjs/operators'
// Custom
import { BankAccountHttpService } from '../services/bankAccount-http.service'
import { ListResolved } from '../../../../../shared/classes/list-resolved'

@Injectable({ providedIn: 'root' })

export class BankAccountListResolver {

    constructor(private bankAccountHttpService: BankAccountHttpService) { }

    resolve(): Observable<ListResolved> {
        return this.bankAccountHttpService.getAll().pipe(
            map((list) => new ListResolved(list)),
            catchError((err: any) => of(new ListResolved(null, err)))
        )
    }

}
