import { Metadata } from 'src/app/shared/classes/metadata'

export interface DestinationReadDto extends Metadata {

    id: number
    abbreviation: string
    description: string
    linkedId: number
    isLinkTwist: boolean
    linkTwistAlias: string
    isActive: boolean

}
