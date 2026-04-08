import { Guid } from 'guid-typescript'
// Custom
import { PassengerImportDto } from './passenger-import-dto'

export interface ReservationImportDto {

    reservationId: Guid
    linkTwistId: string
    date: string
    customerId: number
    destinationId: number
    pickupPointId: number
    ticketNo: string
    email: string
    phones: string
    adults: number
    kids: number
    free: number
    notes: string
    passengers: PassengerImportDto[]

}
