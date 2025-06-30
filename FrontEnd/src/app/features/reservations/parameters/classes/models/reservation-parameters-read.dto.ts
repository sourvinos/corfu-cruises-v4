import { Metadata } from 'src/app/shared/classes/metadata'

export interface ReservationParametersReadDto extends Metadata {

    // PK
    id: number
    // Fields
    closingTime: string
    phones: string
    email: string

}
