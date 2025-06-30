import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface LedgerCriteriaVM {

    fromDate: string
    toDate: string
    selectedCustomers: SimpleEntity[]
    selectedDestinations: SimpleEntity[]
    selectedPorts: SimpleEntity[]
    selectedShips: SimpleEntity[]

}