import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class InvoiceHttpJsonService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl)
    }

    public get(invoiceId: string): Observable<any> {
        return this.http.get(this.url + '/invoicesJson/getById/' + invoiceId)
    }

    public download(invoiceId: string): Observable<any> {
        return this.http.get(this.url + '/invoicesJson/downloadById/' + invoiceId)
    }

}

