import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { GenderAutoCompleteVM } from '../view-models/gender-autocomplete-vm'
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class GenderService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/genders')
    }

    //#region public methods

    public getForBrowser(): Observable<GenderAutoCompleteVM[]> {
        return this.http.get<GenderAutoCompleteVM[]>(environment.apiUrl + '/genders/getForBrowser')
    }

    //#endregion

}
