import { SimpleEntity } from '../../../../../../shared/classes/simple-entity'

export interface ReceiptListVM {

    invoiceId: string
    date: SimpleEntity
    formattedDate: string
    customer: SimpleEntity
    documentType: SimpleEntity
    shipOwner: SimpleEntity
    paymentMethod: SimpleEntity
    invoiceNo: number
    grossAmount: number
    remarks: string
    isEmailPending: boolean
    isEmailSent: boolean
    isCancelled: boolean

}
