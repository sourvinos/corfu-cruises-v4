import { Injectable } from '@angular/core'
// Custom
import { AadeVM } from '../view-models/form/aade-vm'

@Injectable({ providedIn: 'root' })

export class InvoiceJsonHelperService {

    public processInvoiceResponse(invoiceId: string, response: any): any {
        if (response.body.response == '') {
            const z: AadeVM = {
                invoiceId: invoiceId,
                id: '',
                uId: '',
                mark: '',
                markCancel: '',
                authenticationCode: '',
                iCode: '',
                url: '',
                discriminator: 'oxygen'
            }
            return z
        }
        const x = JSON.parse(response.body.response)
        if ('message' in x) {
            const z: AadeVM = {
                invoiceId: invoiceId,
                id: '',
                uId: '',
                mark: '',
                markCancel: '',
                authenticationCode: '',
                iCode: '',
                url: '',
                discriminator: 'oxygen'
            }
            return z
        } else {
            const z: AadeVM = {
                invoiceId: invoiceId,
                id: x.id,
                uId: x.uid,
                mark: x.mark,
                markCancel: '',
                authenticationCode: x.authentication_code,
                iCode: x.icode,
                url: x.url,
                discriminator: 'oxygen'
            }
            return z
        }
    }

}
