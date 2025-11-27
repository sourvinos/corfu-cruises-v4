export interface DestinationWriteDto {

    // PK
    id: number
    abbreviation: string
    description: string
    linkTwistAlias: string
    isActive: boolean
    // Rowversion
    putAt: string

}
