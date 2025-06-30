import { Guid } from 'guid-typescript'
// Custom
import { PortWriteDto } from './port-write-dto'

export interface InvoiceWriteDto {

    invoiceId: Guid
    customerId: number
    destinationId: number
    documentTypeId: number
    paymentMethodId: number
    shipId: number
    date: string
    tripDate: string
    invoiceNo: number
    netAmount: number
    vatPercent: number
    vatAmount: number
    grossAmount: number
    invoicesPorts: PortWriteDto[]
    remarks: string
    putAt: string

}
