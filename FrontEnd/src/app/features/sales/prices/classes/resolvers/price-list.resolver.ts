import { Injectable } from '@angular/core'
import { Observable, of } from 'rxjs'
import { catchError, map } from 'rxjs/operators'
// Custom
import { ListResolved } from '../../../../../shared/classes/list-resolved'
import { PriceHttpService } from '../services/price-http.service'

@Injectable({ providedIn: 'root' })

export class PriceListResolver {

    constructor(private priceService: PriceHttpService) { }

    resolve(): Observable<ListResolved> {
        return this.priceService.getAll()
            .pipe(
                map((priceList) => new ListResolved(priceList)),
                catchError((err: any) => of(new ListResolved(null, err)))
            )
    }

}
