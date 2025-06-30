import { ManifestNationalityVM } from './manifest-nationality-vm'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface ManifestCrewVM {

    id: number
    lastname: string
    firstname: string
    birthdate: string
    phones: string
    gender: SimpleEntity
    nationality: ManifestNationalityVM
    port: SimpleEntity
    specialty: SimpleEntity

}
