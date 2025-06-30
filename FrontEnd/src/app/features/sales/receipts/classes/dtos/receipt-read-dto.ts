import { Guid } from 'guid-typescript'
// Custom
import { DocumentTypeVM } from '../view-models/form/documentType-vm'
import { Metadata } from 'src/app/shared/classes/metadata'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface ReceiptReadDto extends Metadata {

    // PK
    invoiceId: Guid
    // Fields
    date: string
    tripDate: string
    invoiceNo: number
    customer: SimpleEntity
    documentType: DocumentTypeVM
    paymentMethod: SimpleEntity
    shipOwner: SimpleEntity
    grossAmount: number
    remarks: string
    isEmailSent: boolean,
    isCancelled: boolean,
    // Metadata
    postAt: string
    postUser: string
    putAt: string
    putUser: string

}
