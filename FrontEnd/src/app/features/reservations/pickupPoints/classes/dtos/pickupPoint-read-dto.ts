import { CoachRouteAutoCompleteVM } from '../../../coachRoutes/classes/view-models/coachRoute-autocomplete-vm'
import { Metadata } from 'src/app/shared/classes/metadata'
import { PortAutoCompleteVM } from '../../../ports/classes/view-models/port-autocomplete-vm'

export interface PickupPointReadDto extends Metadata {

    // PK
    id: number
    // Object fields
    coachRoute: CoachRouteAutoCompleteVM
    port: PortAutoCompleteVM
    // Fields
    description: string
    linkTwistAlias: string
    exactPoint: string
    time: string
    remarks: string
    isTemp: boolean
    isActive: boolean

}
