import { Guid } from 'guid-typescript'

export interface PriceWriteDto {

    // PK
    id: Guid
    // FKs
    customerId: number
    destinationId: number
    portId: number
    // Fields
    from: string
    to: string
    adultsWithTransfer: number
    adultsWithoutTransfer: number
    kidsWithTransfer: number
    kidsWithoutTransfer: number
    // Rowversion
    putAt: string

}
