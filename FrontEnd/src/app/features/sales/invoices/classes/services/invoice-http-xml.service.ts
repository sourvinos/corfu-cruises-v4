import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class InvoiceHttpXmlService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/invoicesXml')
    }

    public get(invoiceId: string): Observable<any> {
        return this.http.get(this.url + '/' + invoiceId)
    }

    public uploadInvoice(invoice: any): Observable<any> {
        return this.http.post<any>(this.url + '/uploadInvoice', invoice)
    }

    public cancelInvoice(invoice: any): Observable<any> {
        return this.http.post<any>(this.url + '/cancelInvoice', invoice)
    }

}

