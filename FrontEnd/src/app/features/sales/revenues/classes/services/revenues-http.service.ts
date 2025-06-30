import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { RevenuesCriteriaVM } from '../view-models/criteria/revenues-criteria-vm'
import { RevenuesVM } from '../view-models/list/revenues-vm'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class RevenuesHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/revenues')
    }

    get(criteria: RevenuesCriteriaVM): Observable<RevenuesVM> {
        return this.http.request<RevenuesVM>('post', this.url + '/buildRevenues', { body: criteria })
    }

}
