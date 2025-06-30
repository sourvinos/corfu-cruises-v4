import { Guid } from 'guid-typescript'

export interface TaxOfficeWriteDto {

    // PK
    id: Guid
    // Fields
    description: string
    isActive: boolean
    // Rowversion
    putAt: string

}
