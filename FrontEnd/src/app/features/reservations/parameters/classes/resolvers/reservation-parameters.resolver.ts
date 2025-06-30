import { Injectable } from '@angular/core'
import { catchError, map, of } from 'rxjs'
// Custom
import { FormResolved } from 'src/app/shared/classes/form-resolved'
import { ReservationParametersHttpService } from '../services/reservation-parameters-http.service'

@Injectable({ providedIn: 'root' })

export class ReservationParametersResolver {

    constructor(private reservationParametersHttpService: ReservationParametersHttpService) { }

    resolve(): any {
        return this.reservationParametersHttpService.get().pipe(
            map((parameters) => new FormResolved(parameters)),
            catchError((err: any) => of(new FormResolved(null, err)))
        )
    }

}
