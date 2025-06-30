import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface DocumentTypeAutoCompleteVM {

    id: number
    abbreviation: string
    description: string
    ship: SimpleEntity
    batch: string
    batchEn: string

}
