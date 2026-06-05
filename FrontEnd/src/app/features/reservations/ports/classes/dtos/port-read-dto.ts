import { Metadata } from 'src/app/shared/classes/metadata'

export interface PortReadDto extends Metadata {

    id: number
    abbreviation: string
    description: string
    locode: string
    stopOrder: number
    isShownInCriteria: boolean
    isActive: boolean

}
