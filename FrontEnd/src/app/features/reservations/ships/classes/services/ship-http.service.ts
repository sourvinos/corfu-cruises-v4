import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { ShipAutoCompleteVM } from '../view-models/ship-autocomplete-vm'
import { SimpleCriteriaEntity } from 'src/app/shared/classes/simple-criteria-entity'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class ShipHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/ships')
    }

    //#region public methods

    getForBrowser(): Observable<ShipAutoCompleteVM[]> {
        return this.http.get<ShipAutoCompleteVM[]>(environment.apiUrl + '/ships/getForBrowser')
    }

    public getForCriteria(): Observable<SimpleCriteriaEntity[]> {
        return this.http.get<SimpleCriteriaEntity[]>(environment.apiUrl + '/ships/getForCriteria')
    }

    //#endregion

}
