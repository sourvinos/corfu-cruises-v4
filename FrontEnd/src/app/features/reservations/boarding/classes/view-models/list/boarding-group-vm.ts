import { BoardingReservationVM } from './boarding-reservation-vm'

export interface BoardingGroupVM {

    totalPax: number
    embarkedPassengers: number
    pendingPax: number

    reservations: BoardingReservationVM[]

}
