import { Metadata } from 'src/app/shared/classes/metadata'

export interface DestinationReadDto extends Metadata {

    // PK
    id: number
    // Fields
    abbreviation: string
    description: string
    isActive: boolean

}
