import { Guid } from 'guid-typescript'
// Custom
import { NationalityDropdownVM } from './../../../../nationalities/classes/view-models/nationality-autocomplete-vm'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface PassengerReadDto {

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
    isBoarded: boolean

}
