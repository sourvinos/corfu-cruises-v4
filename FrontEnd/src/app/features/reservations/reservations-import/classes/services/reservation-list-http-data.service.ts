import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { ReservationImportListCriteriaVM } from '../view-models/criteria/reservations-import-list-criteria-vm'
import { ReservationImportListVM } from '../view-models/list/reservation-import-list-vm'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class ReservationImportHttpDataService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl)
    }

    public getForList(criteria: ReservationImportListCriteriaVM): Observable<ReservationImportListVM[]> {
        return this.http.request<ReservationImportListVM[]>('post', environment.apiUrl + '/reservationsLinkTwist', { body: criteria })
    }

}

