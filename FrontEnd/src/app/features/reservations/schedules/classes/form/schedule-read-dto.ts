import { DestinationAutoCompleteVM } from '../../../destinations/classes/view-models/destination-autocomplete-vm'
import { Metadata } from 'src/app/shared/classes/metadata'
import { PortAutoCompleteVM } from '../../../ports/classes/view-models/port-autocomplete-vm'

export interface ScheduleReadDto extends Metadata {

    // PK
    id: number
    // Object fields
    destination: DestinationAutoCompleteVM
    port: PortAutoCompleteVM
    // Fields
    date: string
    maxPax: number
    time: string
    isActive: boolean

}
