import { Metadata } from 'src/app/shared/classes/metadata'
import { ShipOwnerBrowserStorageVM } from '../../../shipOwners/classes/view-models/shipOwner-autocomplete-vm'

export interface ShipReadDto extends Metadata {

    // PK
    id: number
    // Object fields
    shipOwner: ShipOwnerBrowserStorageVM
    // Fields
    abbreviation: string
    description: string
    registryNo: string
    isActive: boolean

}
