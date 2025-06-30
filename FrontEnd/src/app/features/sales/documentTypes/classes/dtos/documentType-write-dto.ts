export interface DocumentTypeWriteDto {

    // PK
    id: number
    // FKs
    shipId?: number
    shipOwnerId: number
    // Fields
    abbreviation: string
    abbreviationEn: string
    description: string
    batch: string
    batchEn: string
    customers: string
    suppliers: string
    discriminatorId: number
    isMyData: boolean
    table8_1: string
    table8_8: string
    table8_9: string
    isDefault: boolean
    isActive: boolean
    // Metadata
    putAt: string

}
