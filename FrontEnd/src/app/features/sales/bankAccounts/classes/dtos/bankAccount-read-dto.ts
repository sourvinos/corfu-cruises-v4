import { Metadata } from 'src/app/shared/classes/metadata'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface BankAccountReadDto extends Metadata {

    // PK
    id: number
    // Object fields
    shipOwner: SimpleEntity
    bank: SimpleEntity
    // Fields
    iban: string
    isActive: boolean

}
