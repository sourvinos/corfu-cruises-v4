import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface PickupPointAutoCompleteVM  {

    id: number
    description: string
    exactPoint: string
    time: string
    port: SimpleEntity
    isActive: boolean

}
