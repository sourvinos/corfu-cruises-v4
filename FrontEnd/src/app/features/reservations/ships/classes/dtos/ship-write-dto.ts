export interface ShipWriteDto {

    // PK
    id: number
    // FKs
    shipOwnerId: number
    // Fields
    abbreviation: string
    description: string
    registryNo: string
    isActive: boolean
    // Rowversion
    putAt: string

}
