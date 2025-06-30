import { ShipOwnerBrowserStorageVM } from './shipOwner-browser-storage-vm'

export interface ShipBrowserStorageVM {

    id: number
    description: string
    shipOwner: ShipOwnerBrowserStorageVM
    isActive: boolean

}
