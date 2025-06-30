import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { EmailLedgerVM } from '../view-models/email/email-ledger-vm'
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { LedgerPdfCriteriaVM } from '../view-models/pdf/ledger-pdf-criteria-vm'
import { LedgerVM } from '../view-models/list/ledger-vm'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class LedgerHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/ledgerssales')
    }

    get(criteria: LedgerPdfCriteriaVM): Observable<LedgerVM[]> {
        return this.http.request<LedgerVM[]>('post', this.url + '/buildLedger', { body: criteria })
    }

    buildPdf(criteria: LedgerPdfCriteriaVM): Observable<any> {
        return this.http.request<LedgerVM[]>('post', this.url + '/buildLedgerPdf', { body: criteria })
    }

    emailLedger(criteria: EmailLedgerVM): Observable<any> {
        return this.http.request<EmailLedgerVM[]>('post', this.url + '/emailLedger', { body: criteria })
    }

    openPdf(filename: string): Observable<any> {
        return this.http.get(this.url + '/openPdf/' + filename, { responseType: 'arraybuffer' })
    }

}
