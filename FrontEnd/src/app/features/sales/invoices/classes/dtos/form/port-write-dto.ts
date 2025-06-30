import { Guid } from 'guid-typescript'

export interface PortWriteDto {

    invoiceId: Guid
    portId: number
    adultsWithTransfer: number
    adultsPriceWithTransfer: number
    adultsWithoutTransfer: number
    adultsPriceWithoutTransfer: number
    kidsWithTransfer: number
    kidsPriceWithTransfer: number
    kidsWithoutTransfer: number
    kidsPriceWithoutTransfer: number
    freeWithTransfer: number
    freeWithoutTransfer: number

}
