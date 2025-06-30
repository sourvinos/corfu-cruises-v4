export interface DriverWriteDto {

    // PK
    id: number
    // Fields
    description: string
    phones: string
    isActive: boolean
    // Rowversion
    putAt: string

}
