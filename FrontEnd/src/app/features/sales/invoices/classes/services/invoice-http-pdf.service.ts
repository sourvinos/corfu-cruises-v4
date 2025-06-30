import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class InvoiceHttpPdfService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/invoicesPdf')
    }

    public buildPdf(invoiceIds: string[]): Observable<any> {
        return this.http.post<any>(this.url + '/buildInvoicePdfs', invoiceIds)
    }

    public openPdf(filename: string): Observable<any> {
        return this.http.get(this.url + '/openPdf/' + filename, { responseType: 'arraybuffer' })
    }

}

