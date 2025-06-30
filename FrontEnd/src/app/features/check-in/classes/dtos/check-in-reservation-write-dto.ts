import { Guid } from 'guid-typescript'
// Custom
import { CheckInPassengerWriteDto } from './check-in-passenger-write-dto'

export interface CheckInReservationWriteDto {

    // PK
    reservationId: Guid
    // Fields
    destinationId: number,
    pickupPointId: number,
    portId: number,
    date: string,
    passengers: CheckInPassengerWriteDto[]
    putAt: string

}
