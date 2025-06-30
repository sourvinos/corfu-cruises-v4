import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { CustomerAadeRequestVM } from '../view-models/customer-aade-request-vm'
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class CustomerAadeHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/customersAade')
    }

    public searchRegistry(vm: CustomerAadeRequestVM): Observable<any> {
        return this.http.post(environment.apiUrl + '/customersAade/searchRegistry', vm)
    }

}
