import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { ShipOwnerAadeRequestVM } from '../view-models/shipOwner-aade-vm'
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class ShipOwnerAadeHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/shipOwnersAade')
    }

    public searchRegistry(vm: ShipOwnerAadeRequestVM): Observable<any> {
        return this.http.post(environment.apiUrl + '/shipOwnersAade/searchRegistry', vm)
    }

}
