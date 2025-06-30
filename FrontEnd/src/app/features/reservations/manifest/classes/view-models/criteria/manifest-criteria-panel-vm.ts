import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface ManifestCriteriaPanelVM {

    date: string
    selectedDestinations: SimpleEntity[]
    selectedPorts: SimpleEntity[]
    selectedShips: SimpleEntity[]

}