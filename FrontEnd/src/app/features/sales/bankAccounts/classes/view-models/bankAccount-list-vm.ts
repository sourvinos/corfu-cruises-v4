import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface BankAccountListVM {

    id: number
    shipOwner: SimpleEntity
    bank: SimpleEntity
    iban: string
    isActive: boolean

}
