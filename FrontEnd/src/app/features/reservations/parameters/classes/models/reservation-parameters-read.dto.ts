import { Metadata } from 'src/app/shared/classes/metadata'

export interface ReservationParametersReadDto extends Metadata {

    id: number
    closingTime: string
    phones: string
    email: string
    linkTwistIsDemo: boolean
    linkTwistDemoUrl: string
    linkTwistDemoAPIKey: string
    linkTwistLiveUrl: string
    linkTwistLiveAPIKey: string

}
