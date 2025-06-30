import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { ManifestCrewVM } from '../view-models/list/manifest-crew-vm'
import { ManifestPassengerVM } from '../view-models/list/manifest-passenger-vm'
import { ManifestSearchCriteriaVM } from '../view-models/criteria/manifest-search-criteria-vm'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class ManifestHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/manifest')
    }

    getPassengers(criteria: ManifestSearchCriteriaVM): Observable<ManifestPassengerVM> {
        return this.http.request<ManifestPassengerVM>('post', this.url, { body: criteria })
    }

    getCrew(shipId: number): Observable<ManifestCrewVM[]> {
        return this.http.get<ManifestCrewVM[]>(this.url + '/' + shipId)
    }

}
