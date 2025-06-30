import { Guid } from 'guid-typescript'
// Custom
import { AadeVM } from '../../view-models/form/aade-vm'
import { DocumentTypeVM } from '../../view-models/form/documentType-vm'
import { InvoiceXmlPartyTypeDto } from '../xml/invoice-xml-partyType-dto'
import { Metadata } from 'src/app/shared/classes/metadata'
import { PortReadDto } from './port-read-dto'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface InvoiceReadDto extends Metadata {

    invoiceId: Guid
    date: string
    tripDate: string
    invoiceNo: number
    customer: SimpleEntity
    destination: SimpleEntity
    documentType: DocumentTypeVM
    paymentMethod: SimpleEntity
    ship: SimpleEntity
    aade: AadeVM
    issuer: InvoiceXmlPartyTypeDto
    counterPart: InvoiceXmlPartyTypeDto
    invoicesPorts: PortReadDto[]
    remarks: string
    isEmailSent: boolean
    isCancelled: boolean
    adults: number
    kids: number
    free: number
    totalPax: number
    netAmount: number
    vatPercent: number
    vatAmount: number
    grossAmount: number

}
