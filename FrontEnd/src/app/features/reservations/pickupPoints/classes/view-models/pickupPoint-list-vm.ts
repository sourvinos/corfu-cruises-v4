import { PickupPointListCoachRouteVM } from './pickupPoint-list-coachRoute-vm'
import { PickupPointListPortVM } from './pickupPoint-list-port-vm'

export interface PickupPointListVM {

    id: number
    description: string
    coachRoute: PickupPointListCoachRouteVM
    port: PickupPointListPortVM
    exactPoint: string
    time: string
    isActive: boolean

}
