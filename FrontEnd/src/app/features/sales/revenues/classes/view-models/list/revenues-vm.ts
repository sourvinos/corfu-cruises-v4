import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface RevenuesVM {

    customer: SimpleEntity
    previous: number
    debit: number
    credit: number
    periodBalance: number
    total: number

}
