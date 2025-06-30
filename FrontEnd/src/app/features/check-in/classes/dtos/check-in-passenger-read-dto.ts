import { Guid } from 'guid-typescript'
import { NationalityDropdownVM } from 'src/app/features/reservations/nationalities/classes/view-models/nationality-autocomplete-vm'
// Custom
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface CheckInPassengerReadDto {

    id: number
    reservationId: Guid
    gender: SimpleEntity
    nationality: NationalityDropdownVM
    occupant: SimpleEntity
    lastname: string
    firstname: string
    birthdate: string
    remarks: string
    specialCare: string

}
