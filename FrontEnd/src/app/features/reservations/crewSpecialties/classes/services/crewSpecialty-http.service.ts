import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { CrewSpecialtyAutoCompleteVM } from '../view-models/crewSpecialty-autocomplete-vm'
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class CrewSpecialtyHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/crewSpecialties')
    }

    //#region public methods

    getForBrowser(): Observable<CrewSpecialtyAutoCompleteVM[]> {
        return this.http.get<CrewSpecialtyAutoCompleteVM[]>(environment.apiUrl + '/crewSpecialties/getForBrowser')
    }

    //#endregion

}
