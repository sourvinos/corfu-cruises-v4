import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { EmailReceiptVM } from '../view-models/email/email-receipt-vm'
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { ReceiptListCriteriaVM } from '../view-models/criteria/receipt-list-criteria-vm'
import { ReceiptListVM } from '../view-models/list/receipt-list-vm'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class ReceiptHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/receipts')
    }

    public getForList(criteria: ReceiptListCriteriaVM): Observable<ReceiptListVM[]> {
        return this.http.request<ReceiptListVM[]>('post', environment.apiUrl + '/receipts/getForPeriod', { body: criteria })
    }

    public buildPdf(invoiceIds: string[]): Observable<any> {
        return this.http.post<any>(this.url + '/buildReceiptPdfs', invoiceIds)
    }

    public openPdf(filename: string): Observable<any> {
        return this.http.get(this.url + '/openPdf/' + filename, { responseType: 'arraybuffer' })
    }

    public emailReceipts(criteria: EmailReceiptVM): Observable<any> {
        return this.http.request<EmailReceiptVM[]>('post', this.url + '/emailReceipts', { body: criteria })
    }

    public patchInvoiceWithIsCancelled(invoiceId: string): Observable<any> {
        return this.http.patch<any>(this.url + '/isCancelled/' + invoiceId, null)
    }

    public patchReceiptsWithEmailPending(invoiceIds: string[]): Observable<any> {
        return this.http.patch<any>(this.url + '/patchReceiptsWithEmailPending', invoiceIds)
    }

    public patchReceiptWithEmailSent(invoiceIds: string[]): Observable<any> {
        return this.http.patch<any>(this.url + '/patchReceiptsWithEmailSent', invoiceIds)
    }

    public override save(formData: any): Observable<any> {
        return formData.invoiceId == null
            ? this.http.post<any>(this.url, formData)
            : this.http.put<any>(this.url, formData)
    }

}
