import { InvoiceXmlAddressDto } from './invoice-xml-address-dto'

export interface InvoiceXmlPartyTypeDto {

    vatNumber: string
    country: string
    branch: number,
    address: InvoiceXmlAddressDto

}
