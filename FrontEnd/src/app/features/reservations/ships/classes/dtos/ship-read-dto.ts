import { Metadata } from 'src/app/shared/classes/metadata'
import { ShipOwnerBrowserStorageVM } from '../../../shipOwners/classes/view-models/shipOwner-autocomplete-vm'

export interface ShipReadDto extends Metadata {

    id: number
    shipOwner: ShipOwnerBrowserStorageVM
    abbreviation: string
    description: string
    registryNo: string
    isShownInCriteria: boolean
    isActive: boolean

}
