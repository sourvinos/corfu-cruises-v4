import { Injectable } from '@angular/core'
import { Observable, of } from 'rxjs'
import { catchError, map } from 'rxjs/operators'
// Custom
import { LedgerHttpService } from '../services/ledger-http.service'
import { ListResolved } from '../../../../../shared/classes/list-resolved'

@Injectable({ providedIn: 'root' })

export class LedgerListResolver {

    constructor(private ledgerHttpService: LedgerHttpService) { }

    resolve(): Observable<ListResolved> {
        return this.ledgerHttpService.getAll()
            .pipe(
                map((ledgerList) => new ListResolved(ledgerList)),
                catchError((err: any) => of(new ListResolved(null, err)))
            )
    }

}
