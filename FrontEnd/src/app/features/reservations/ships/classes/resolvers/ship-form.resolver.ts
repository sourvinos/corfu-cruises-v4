import { ActivatedRouteSnapshot } from '@angular/router'
import { Injectable } from '@angular/core'
import { catchError, map, of } from 'rxjs'
// Custom
import { FormResolved } from 'src/app/shared/classes/form-resolved'
import { ShipHttpService } from '../services/ship-http.service'

@Injectable({ providedIn: 'root' })

export class ShipFormResolver {

    constructor(private shipService: ShipHttpService) { }

    resolve(route: ActivatedRouteSnapshot): any {
        return this.shipService.getSingle(route.params.id).pipe(
            map((shipForm) => new FormResolved(shipForm)),
            catchError((err: any) => of(new FormResolved(null, err)))
        )
    }

}
