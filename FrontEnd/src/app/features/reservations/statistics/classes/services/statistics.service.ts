import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class StatisticsService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/statistics')
    }

    public getStatistics(year: string, table: string): Observable<any> {
        return this.http.get<any>(environment.apiUrl + '/statistics/' + table + '/year/' + year)
    }

}
