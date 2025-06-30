export interface BankAccountWriteDto {

    // PK
    id: number
    // FKs
    shipOwnerId: number
    bankId: number
    // Fields
    iban: string
    isActive: boolean
    // Rowversion
    putAt: string

}
