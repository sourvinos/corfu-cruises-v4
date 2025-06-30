import { Guid } from 'guid-typescript'
// Custom
import { CustomerAutoCompleteVM } from '../../../customers/classes/view-models/customer-autocomplete-vm'
import { Metadata } from 'src/app/shared/classes/metadata'

export interface UserReadDto extends Metadata {

    id: Guid
    username: string
    displayname: string
    customer: CustomerAutoCompleteVM
    email: string
    isFirstFieldFocused: boolean
    isAdmin: boolean
    isActive: boolean

}
