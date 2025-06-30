import { Observable } from 'rxjs'
import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
// Custom
import { HttpDataService } from './http-data.service'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class CompanyDataService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl)
    }

    //#region public methods

    public getCompanyData(url: string): Observable<any> {
        return this.http.get<any>(url + '/parameters')
    }

}
