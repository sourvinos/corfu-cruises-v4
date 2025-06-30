import { InvoiceListAadeVM } from './invoice-list-aade-vm'
import { SimpleEntity } from '../../../../../../shared/classes/simple-entity'

export interface InvoiceListVM {

    customer: SimpleEntity
    date: SimpleEntity
    destination: SimpleEntity
    documentType: SimpleEntity
    batch: string
    grossAmount: number
    invoiceId: string
    invoiceNo: number
    formattedDate: string
    isEmailPending: boolean
    isEmailSent: boolean
    ship: SimpleEntity
    shipOwner: SimpleEntity
    aade: InvoiceListAadeVM

}
