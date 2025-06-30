import { SimpleEntity } from './../../../../../shared/classes/simple-entity'

export interface PriceListVM {

    id: number
    customer: SimpleEntity
    destination: SimpleEntity
    port: SimpleEntity
    from: string
    formattedFrom: string
    to: string
    formattedTo: string
    adultsWithTransfer: number
    adultsWithoutTransfer: number
    kidsWithTransfer: number
    kidsWithoutTransfer: number

}
