import { Injectable } from '@angular/core'
import { Observable, of } from 'rxjs'
import { catchError, map } from 'rxjs/operators'
import { ListResolved } from '../../../../../shared/classes/list-resolved'
import { PickupPointHttpService } from '../services/pickupPoint-http.service'

@Injectable({ providedIn: 'root' })

export class PickupPointListResolver {

    constructor(private pickupPointService: PickupPointHttpService) { }

    resolve(): Observable<ListResolved> {
        return this.pickupPointService.getAll()
            .pipe(
                map((pickupPointList) => new ListResolved(pickupPointList)),
                catchError((err: any) => of(new ListResolved(null, err)))
            )
    }

}
