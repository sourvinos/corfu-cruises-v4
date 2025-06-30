export interface PickupPointWriteDto {

    // PK
    id: number
    // FKs
    coachRouteId: number
    portId: number
    // Fields
    description: string
    exactPoint: string
    time: string
    remarks: string
    isActive: boolean
    // Rowversion
    putAt: string

}
