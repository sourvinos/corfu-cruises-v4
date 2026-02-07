import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { StatisticsCriteriaVM } from '../view-models/criteria/statistics-criteria-vm'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class StatisticsHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/statistics')
    }

    public getStatistics(table: string, criteria: StatisticsCriteriaVM): Observable<any> {
        return this.http.post<any>(environment.apiUrl + '/statistics/' + table, criteria)
    }

}
