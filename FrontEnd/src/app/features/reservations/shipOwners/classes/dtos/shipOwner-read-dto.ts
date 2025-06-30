import { Metadata } from 'src/app/shared/classes/metadata'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface ShipOwnerReadDto extends Metadata {

    // PK
    id: number
    // Object fields
    nationality: SimpleEntity
    taxOffice: SimpleEntity
    // Fields
    vatPercent: number
    vatPercentId: number
    vatExemptionId: number
    description: string
    descriptionEn: string
    vatNumber: string
    branch: number
    profession: string
    street: string
    number: string
    postalCode: string
    city: string
    personInCharge: string
    phones: string
    email: string
    isGroupJP: boolean
    // myData
    myDataDemoUrl: string
    myDataDemoUsername: string
    myDataDemoSubscriptionKey: string
    myDataLiveUrl: string
    myDataLiveUsername: string
    myDataLiveSubscriptionKey: string
    myDataIsDemo: boolean
    myDataIsActive: boolean
    // Oxygen
    oxygenDemoUrl: string
    oxygenDemoApiKey: string
    oxygenLiveUrl: string
    oxygenLiveApiKey: string
    oxygenIsDemo: boolean
    oxygenIsActive: boolean
    isActive: boolean

}
