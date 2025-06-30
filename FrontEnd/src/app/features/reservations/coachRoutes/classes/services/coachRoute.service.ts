import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { CoachRouteAutoCompleteVM } from '../view-models/coachRoute-autocomplete-vm'
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class CoachRouteService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/coachRoutes')
    }

    //#region public methods

    public getForBrowser(): Observable<any[]> {
        return this.http.get<CoachRouteAutoCompleteVM[]>(environment.apiUrl + '/coachRoutes/getForBrowser')
    }

    //#endregion

}
