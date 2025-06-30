import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { ShipOwnerBrowserStorageVM } from '../view-models/shipOwner-autocomplete-vm'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class ShipOwnerHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/shipOwners')
    }

    public getForBrowser(): Observable<ShipOwnerBrowserStorageVM[]> {
        return this.http.get<ShipOwnerBrowserStorageVM[]>(environment.apiUrl + '/shipOwners/getForBrowser')
    }

}
