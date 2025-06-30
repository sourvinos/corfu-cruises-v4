import { Guid } from 'guid-typescript'
// Custom
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface PortReadDto {

    id: number
    invoiceId: Guid
    port: SimpleEntity
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
    pax: number
    amount: number

}
