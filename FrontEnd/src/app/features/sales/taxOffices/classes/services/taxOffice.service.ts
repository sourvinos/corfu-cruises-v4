import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { TaxOfficeAutoCompleteVM } from '../view-models/taxOffice-autocomplete-vm'
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class TaxOfficeService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/taxOffices')
    }

    //#region public methods

    public getForBrowser(): Observable<TaxOfficeAutoCompleteVM[]> {
        return this.http.get<TaxOfficeAutoCompleteVM[]>(environment.apiUrl + '/taxOffices/getForBrowser')
    }

    //#endregion

}
