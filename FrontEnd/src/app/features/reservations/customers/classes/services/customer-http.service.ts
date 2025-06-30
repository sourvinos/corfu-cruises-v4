import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { CustomerBrowserStorageVM } from '../view-models/customer-browser-storage-vm'
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { SimpleCriteriaEntity } from 'src/app/shared/classes/simple-criteria-entity'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class CustomerHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/customers')
    }

    //#region public methods

    public getBrowserStorage(): Observable<CustomerBrowserStorageVM[]> {
        return this.http.get<CustomerBrowserStorageVM[]>(environment.apiUrl + '/customers/getForBrowser')
    }

    public getForCriteria(): Observable<SimpleCriteriaEntity[]> {
        return this.http.get<SimpleCriteriaEntity[]>(environment.apiUrl + '/customers/getForCriteria')
    }

    //#endregion

}
