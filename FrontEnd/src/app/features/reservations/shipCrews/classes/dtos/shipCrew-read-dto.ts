import { GenderAutoCompleteVM } from '../../../genders/classes/view-models/gender-autocomplete-vm'
import { Metadata } from 'src/app/shared/classes/metadata'
import { NationalityDropdownVM } from '../../../nationalities/classes/view-models/nationality-autocomplete-vm'
import { ShipAutoCompleteVM } from '../../../ships/classes/view-models/ship-autocomplete-vm'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface ShipCrewReadDto extends Metadata {

    // PK
    id: number
    // Object fields
    gender: GenderAutoCompleteVM
    nationality: NationalityDropdownVM
    ship: ShipAutoCompleteVM
    specialty: SimpleEntity
    // Fields
    lastname: string
    firstname: string
    birthdate: string
    isActive: boolean

}
