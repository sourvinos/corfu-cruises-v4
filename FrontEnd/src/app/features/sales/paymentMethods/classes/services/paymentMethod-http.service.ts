import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { PaymentMethodAutoCompleteVM } from '../view-models/paymentMethod-autocomplete-vm'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class PaymentMethodHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/paymentMethods')
    }

    //#region public methods

    public getForBrowser(): Observable<PaymentMethodAutoCompleteVM[]> {
        return this.http.get<PaymentMethodAutoCompleteVM[]>(environment.apiUrl + '/paymentMethods/getForBrowser')
    }

    //#endregion

}
