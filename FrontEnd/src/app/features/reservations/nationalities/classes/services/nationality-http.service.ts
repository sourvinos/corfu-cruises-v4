import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { NationalityDropdownVM } from '../view-models/nationality-autocomplete-vm'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class NationalityHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/nationalities')
    }

    //#region public methods

    getForBrowser(): Observable<NationalityDropdownVM[]> {
        return this.http.get<NationalityDropdownVM[]>(environment.apiUrl + '/nationalities/getForBrowser')
    }

    //#endregion

}
