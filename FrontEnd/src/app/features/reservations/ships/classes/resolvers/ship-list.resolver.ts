import { Injectable } from '@angular/core'
import { Observable, of } from 'rxjs'
import { catchError, map } from 'rxjs/operators'
// Custom
import { ListResolved } from '../../../../../shared/classes/list-resolved'
import { ShipHttpService } from '../services/ship-http.service'

@Injectable({ providedIn: 'root' })

export class ShipListResolver {

    constructor(private shipService: ShipHttpService) { }

    resolve(): Observable<ListResolved> {
        return this.shipService.getAll().pipe(
            map((shipList) => new ListResolved(shipList)),
            catchError((err: any) => of(new ListResolved(null, err)))
        )
    }

}
