import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { AadeVM } from '../view-models/form/aade-vm'
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { InvoiceListCriteriaVM } from '../view-models/criteria/invoice-list-criteria-vm'
import { InvoiceListVM } from '../view-models/list/invoice-list-vm'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class InvoiceHttpDataService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/invoices')
    }

    public getForList(criteria: InvoiceListCriteriaVM): Observable<InvoiceListVM[]> {
        return this.http.request<InvoiceListVM[]>('post', environment.apiUrl + '/invoices/getForPeriod', { body: criteria })
    }

    public validateCustomerData(customerId: number): Observable<any> {
        return this.http.get(environment.apiUrl + '/customers/getByIdForValidation/' + customerId)
    }

    public override save(formData: any): Observable<any> {
        return formData.invoiceId == null
            ? this.http.post<any>(this.url, formData)
            : this.http.put<any>(this.url, formData)
    }

    public updateInvoiceAade(aadeVM: AadeVM): Observable<any> {
        return this.http.put<any>(this.url + '/invoiceAade', aadeVM)
    }

    public updateInvoiceOxygen(aadeVM: AadeVM): Observable<any> {
        return this.http.put<any>(this.url + '/invoiceOxygen', aadeVM)
    }

    public patchInvoicesWithEmailPending(invoiceIds: string[]): Observable<any> {
        return this.http.patch<any>(this.url + '/patchInvoicesWithEmailPending', invoiceIds)
    }

    public patchInvoiceWithIsCancelled(invoiceId: string): Observable<any> {
        return this.http.patch<any>(this.url + '/isCancelled/' + invoiceId, null)
    }

}

