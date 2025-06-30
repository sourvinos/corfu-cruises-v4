import { Injectable } from '@angular/core'
import { Observable, of } from 'rxjs'
import { catchError, map } from 'rxjs/operators'
// Custom
import { ListResolved } from '../../../../../shared/classes/list-resolved'
import { RevenuesHttpService } from '../services/revenues-http.service'

@Injectable({ providedIn: 'root' })

export class RevenuesResolver {

    constructor(private revenuesHttpService: RevenuesHttpService) { }

    resolve(): Observable<ListResolved> {
        return this.revenuesHttpService.getAll()
            .pipe(
                map((revenues) => new ListResolved(revenues)),
                catchError((err: any) => of(new ListResolved(null, err)))
            )
    }

}
