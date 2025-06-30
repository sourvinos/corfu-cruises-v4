import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface DocumentTypeListVM {

    id: number
    shipOwner: SimpleEntity
    ship: SimpleEntity
    description: string
    batch: string
    isActive: boolean
    table8_1: string
    table8_8: string
    table8_9: string

}
