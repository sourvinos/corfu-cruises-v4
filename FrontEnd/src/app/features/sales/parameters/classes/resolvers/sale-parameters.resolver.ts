import { Injectable } from '@angular/core'
import { catchError, map, of } from 'rxjs'
// Custom
import { FormResolved } from 'src/app/shared/classes/form-resolved'
import { SaleParametersHttpService } from '../services/sale-parameters-http.service'

@Injectable({ providedIn: 'root' })

export class SaleParametersResolver {

    constructor(private saleParametersHttpService: SaleParametersHttpService) { }

    resolve(): any {
        return this.saleParametersHttpService.get().pipe(
            map((parameters) => new FormResolved(parameters)),
            catchError((err: any) => of(new FormResolved(null, err)))
        )
    }

}
