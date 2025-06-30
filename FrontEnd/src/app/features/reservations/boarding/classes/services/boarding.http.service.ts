import { HttpClient, HttpParams } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { BoardingReservationVM } from '../view-models/list/boarding-reservation-vm'
import { BoardingSearchCriteriaVM } from '../view-models/criteria/boarding-search-criteria-vm'
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class BoardingHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/boarding')
    }

    get(criteria: BoardingSearchCriteriaVM): Observable<BoardingReservationVM> {
        return this.http.request<BoardingReservationVM>('post', this.url, { body: criteria })
    }

    embarkSinglePassenger(id: number): Observable<any> {
        const params = new HttpParams().set('id', id)
        return this.http.patch(this.url + '/embarkPassenger?', null, { params: params })
    }

    embarkPassengers(ignoreCurrentStatus: boolean, id: number[]): Observable<any> {
        let params = new HttpParams().set('ignoreCurrentStatus', ignoreCurrentStatus)
        params = params.append('id', id[0])
        id.forEach((element, index) => {
            if (index > 0) {
                params = params.append('id', element)
            }
        })
        return this.http.patch(this.url + '/embarkPassengers?', null, { params: params })
    }

}
