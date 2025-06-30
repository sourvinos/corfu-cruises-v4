import { ActivatedRouteSnapshot } from '@angular/router'
import { Injectable } from '@angular/core'
import { catchError, map, of } from 'rxjs'
// Custom
import { FormResolved } from 'src/app/shared/classes/form-resolved'
import { ReceiptHttpService } from '../services/receipt-http.service'

@Injectable({ providedIn: 'root' })

export class ReceiptFormResolver {

    constructor(private receiptHttpService: ReceiptHttpService) { }

    resolve(route: ActivatedRouteSnapshot): any {
        return this.receiptHttpService.getSingle(route.params.id).pipe(
            map((receiptForm) => new FormResolved(receiptForm)),
            catchError((err: any) => of(new FormResolved(null, err)))
        )
    }

}
