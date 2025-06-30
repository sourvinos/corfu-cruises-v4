export interface CustomerWriteDto {

    // PK
    id: number
    // FKs
    nationalityId: number
    taxOfficeId: number
    // Fields
    vatPercent: number
    vatPercentId: number
    vatExemptionId: number
    description: string
    fullDescription: string
    vatNumber: string
    branch: number
    profession: string
    street: string
    number: string
    postalCode: string
    city: string
    personInCharge: string
    phones: string
    email: string
    balanceLimit: number
    paxLimit: number
    remarks: string
    isActive: boolean
    // Metadata
    putAt: string

}
