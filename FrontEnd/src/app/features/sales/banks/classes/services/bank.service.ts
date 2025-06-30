import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { BankAutoCompleteVM } from '../view-models/bank-autocomplete-vm'
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class BankService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/banks')
    }

    //#region public methods

    public getForBrowser(): Observable<BankAutoCompleteVM[]> {
        return this.http.get<BankAutoCompleteVM[]>(environment.apiUrl + '/banks/getForBrowser')
    }

    //#endregion

}
