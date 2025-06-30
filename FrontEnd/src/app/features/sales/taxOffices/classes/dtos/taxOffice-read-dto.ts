import { Guid } from 'guid-typescript'
import { Metadata } from 'src/app/shared/classes/metadata'

export interface TaxOfficeReadDto extends Metadata {

    // PK
    id: Guid
    // Fields
    description: string
    isActive: boolean

}
