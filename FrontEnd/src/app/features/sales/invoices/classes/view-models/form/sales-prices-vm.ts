import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface SalesPricesVM {

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
