import { Metadata } from 'src/app/shared/classes/metadata'

export interface DriverReadDto extends Metadata {

    // PK
    id: number
    // Fields
    description: string
    phones: string
    isActive: boolean

}
