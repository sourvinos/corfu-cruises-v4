import { Injectable } from '@angular/core'
import { Observable, of } from 'rxjs'
import { catchError, map } from 'rxjs/operators'
// Custom
import { ListResolved } from '../../../../../shared/classes/list-resolved'
import { ReservationImportHttpDataService } from '../services/reservation-list-http-data.service'

@Injectable({ providedIn: 'root' })

export class ReservationImportListResolver {

    constructor(private reservationImportHttpService: ReservationImportHttpDataService) { }

    resolve(): Observable<ListResolved> {
        return this.reservationImportHttpService.getAll()
            .pipe(
                map((invoiceList) => new ListResolved(invoiceList)),
                catchError((err: any) => of(new ListResolved(null, err)))
            )
    }

}
