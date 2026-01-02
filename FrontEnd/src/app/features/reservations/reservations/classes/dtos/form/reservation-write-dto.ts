import { Guid } from 'guid-typescript'
// Custom
import { PassengerWriteDto } from './passenger-write-dto'

export interface ReservationWriteDto {

    reservationId: Guid
    customerId: number
    destinationId: number
    driverId?: number
    pickupPointId: number
    portId: number
    portAlternateId: number
    shipId?: number
    date: string
    linkTwistId: string
    refNo: string
    ticketNo: string
    email: string
    phones: string
    adults: number
    kids: number
    free: number
    remarks: string
    passengers: PassengerWriteDto[]
    putAt: string

}
