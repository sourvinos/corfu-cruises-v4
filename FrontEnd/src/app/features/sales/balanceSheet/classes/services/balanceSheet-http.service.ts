import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
import { HttpDataService } from 'src/app/shared/services/http-data.service'
// Custom
import { BalanceSheetCriteriaVM } from '../view-models/criteria/balanceSheet-criteria-vm'
import { BalanceSheetVM } from '../view-models/list/balanceSheet-vm'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class BalanceSheetHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/balanceSheet')
    }

    get(criteria: BalanceSheetCriteriaVM): Observable<BalanceSheetVM> {
        return this.http.request<BalanceSheetVM>('post', this.url + '/buildBalanceSheet', { body: criteria })
    }

}
