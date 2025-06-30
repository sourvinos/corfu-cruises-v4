import { InvoicePdfDocumentTypeVM } from './invoice-pdf-documentType-vm'

export interface InvoicePdfHeaderVM {

    date: string
    tripDate: string
    documentType: InvoicePdfDocumentTypeVM
    invoiceNo: string

}

