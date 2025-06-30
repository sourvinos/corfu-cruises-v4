import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface DocumentTypeBrowserStorageVM {

    id: number
    ship: SimpleEntity
    shipOwner: SimpleEntity
    abbreviation: string
    description: string
    batch: string
    batchEn: string
    isDefault: boolean
    isActive: boolean

}
