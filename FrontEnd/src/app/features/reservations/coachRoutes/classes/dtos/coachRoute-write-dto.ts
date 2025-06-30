export interface CoachRouteWriteDto {

    // PK
    id: number
    // Fields
    abbreviation: string
    description: string
    hasTransfer: boolean
    isActive: boolean
    // Rowversion
    putAt: string

}
