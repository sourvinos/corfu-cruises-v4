import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { PortAutoCompleteVM } from '../view-models/port-autocomplete-vm'
import { SimpleCriteriaEntity } from 'src/app/shared/classes/simple-criteria-entity'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class PortHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/ports')
    }

    //#region public methods

    public getForBrowser(): Observable<PortAutoCompleteVM[]> {
        return this.http.get<PortAutoCompleteVM[]>(environment.apiUrl + '/ports/getForBrowser')
    }

    public getForCriteria(): Observable<SimpleCriteriaEntity[]> {
        return this.http.get<SimpleCriteriaEntity[]>(environment.apiUrl + '/ports/getForCriteria')
    }

    //#endregion

}
