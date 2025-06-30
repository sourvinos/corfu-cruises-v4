import { Guid } from 'guid-typescript'
// Custom
import { CheckInPassengerReadDto } from './check-in-passenger-read-dto'
import { PickupPointVM } from '../view-models/pickupPoint-vm'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface CheckInReservationReadDto {

    reservationId: Guid
    date: string
    refNo: string
    ticketNo: string
    adults: number
    kids: number
    free: number
    totalPax: number
    email: string
    phones: string
    remarks: string
    customer: SimpleEntity
    destination: SimpleEntity
    pickupPoint: PickupPointVM
    passengers: CheckInPassengerReadDto[]
    putAt: string

}

