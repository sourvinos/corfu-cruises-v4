import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface BalanceSheetVM {

    customer: SimpleEntity
    shipOwner: SimpleEntity
    previousBalance: number
    debit: number
    credit: number
    actualBalance: number

}
