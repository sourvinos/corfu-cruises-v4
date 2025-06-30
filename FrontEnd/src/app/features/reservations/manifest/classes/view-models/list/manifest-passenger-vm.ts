import { ManifestNationalityVM } from './manifest-nationality-vm'
import { ManifestPortVM } from './manifest-port-vm'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface ManifestPassengerVM {

    id: number
    refNo: string
    lastname: string
    firstname: string
    birthdate: string
    phones: string
    remarks: string
    specialCare: string
    gender: SimpleEntity
    nationality: ManifestNationalityVM
    port: ManifestPortVM

}
