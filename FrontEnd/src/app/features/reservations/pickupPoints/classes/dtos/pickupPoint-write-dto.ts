export interface PickupPointWriteDto {

    id: number
    coachRouteId: number
    portId: number
    description: string
    linkTwistAlias: string
    exactPoint: string
    time: string
    remarks: string
    isTemp: boolean
    isActive: boolean
    putAt: string

}
