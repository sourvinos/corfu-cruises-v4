import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { PickupPointAutoCompleteVM } from '../view-models/pickupPoint-autocomplete-vm'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class PickupPointService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/pickupPoints')
    }

    getForBrowser(): Observable<any[]> {
        return this.http.get<PickupPointAutoCompleteVM[]>(environment.apiUrl + '/pickupPoints/getForBrowser')
    }

}
