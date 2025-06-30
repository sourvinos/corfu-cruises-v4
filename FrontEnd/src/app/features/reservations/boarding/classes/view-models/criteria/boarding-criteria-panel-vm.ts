import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface BoardingCriteriaPanelVM {

    date: string
    selectedDestinations: SimpleEntity[]
    selectedPorts: SimpleEntity[]
    selectedShips: SimpleEntity[]

}
