import { Injectable } from '@angular/core'
import { Observable, of } from 'rxjs'
import { catchError, map } from 'rxjs/operators'
// Custom
import { BalanceSheetHttpService } from '../services/balanceSheet-http.service'
import { ListResolved } from '../../../../../shared/classes/list-resolved'

@Injectable({ providedIn: 'root' })

export class BalanceSheetResolver {

    constructor(private balanceSheetHttpService: BalanceSheetHttpService) { }

    resolve(): Observable<ListResolved> {
        return this.balanceSheetHttpService.getAll()
            .pipe(
                map((balanceSheet) => new ListResolved(balanceSheet)),
                catchError((err: any) => of(new ListResolved(null, err)))
            )
    }

}
