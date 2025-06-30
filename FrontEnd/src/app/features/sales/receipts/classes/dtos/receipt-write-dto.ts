import { Guid } from 'guid-typescript'

export interface ReceiptWriteDto {

    // PK
    invoiceId: Guid
    // FKs
    customerId: number
    documentTypeId: number
    paymentMethodId: number
    shipOwnerId: number
    // Fields
    date: string
    tripDate: string
    invoiceNo: number
    grossAmount: number
    remarks: string
    isEmailSent: boolean
    isCancelled: boolean
    // Metadata
    putAt: string

}
