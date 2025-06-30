import { Guid } from 'guid-typescript'
// Custom
import { Metadata } from 'src/app/shared/classes/metadata'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface PriceReadDto extends Metadata {

    // PK
    id: Guid
    // Object fields
    customer: SimpleEntity
    destination: SimpleEntity
    port: SimpleEntity
    // Fields
    from: string
    to: string
    adultsWithTransfer: number
    adultsWithoutTransfer: number
    kidsWithTransfer: number
    kidsWithoutTransfer: number

}
