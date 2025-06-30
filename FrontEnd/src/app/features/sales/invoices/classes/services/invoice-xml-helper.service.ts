import { Injectable } from '@angular/core'
// Custom
import { AadeVM } from '../view-models/form/aade-vm'

@Injectable({ providedIn: 'root' })

export class InvoiceXmlHelperService {

    public processInvoiceSuccessResponse(response: any): AadeVM {
        const document = new DOMParser().parseFromString(response.body.response, 'text/xml')
        const uId = document.querySelector('invoiceUid').innerHTML
        const mark = document.querySelector('invoiceMark').innerHTML
        const url = document.querySelector('qrUrl').innerHTML
        const x: AadeVM = {
            invoiceId: response.body.invoiceId,
            id: '',
            uId: uId,
            mark: mark,
            markCancel: '',
            authenticationCode: '',
            iCode: '',
            url: url,
            discriminator: 'aade'
        }
        return x
    }

    public processInvoiceCancelSuccessResponse(invoice: any, response: any): AadeVM {
        const document = new DOMParser().parseFromString(response.body.response, 'text/xml')
        const cancellationMark = document.querySelector('cancellationMark').innerHTML
        const x: AadeVM = {
            invoiceId: invoice.invoiceId,
            id: '',
            uId: invoice.aade.uId,
            mark: invoice.aade.mark,
            markCancel: cancellationMark,
            authenticationCode: '',
            iCode: '',
            url: invoice.aade.url,
            discriminator: 'aade'
        }
        return x
    }

}
