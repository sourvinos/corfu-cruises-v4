import { Guid } from 'guid-typescript'
// Custom
import { PassengerWriteDto } from './passenger-write-dto'

export interface ReservationWriteDto {

    // PK
    reservationId: Guid
    // FKs
    customerId: number
    destinationId: number
    driverId?: number
    pickupPointId: number
    portId: number
    portAlternateId: number
    shipId?: number
    // Fields
    date: string
    refNo: string
    ticketNo: string
    email: string
    phones: string
    adults: number
    kids: number
    free: number
    remarks: string
    passengers: PassengerWriteDto[]
    // Rowversion
    putAt: string

}
