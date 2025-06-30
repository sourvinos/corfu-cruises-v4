import { ReservationListCoachRouteVM } from './reservation-list-coachRoute-vm'
import { ReservationListDestinationVM } from './reservation-list-destination-vm'
import { ReservationListDriverVM } from './reservation-list-driver-vm'
import { ReservationListPickupPointVM } from './reservation-list-pickupPoint-vm'
import { ReservationListPortVM } from './reservation-list-port-vm'
import { ReservationListShipVM } from './reservation-list-ship-vm'
import { SimpleEntity } from './../../../../../../shared/classes/simple-entity'

export interface ReservationListVM {

    reservationId: string
    refNo: string
    date: string
    adults: number
    kids: number
    free: number
    totalPax: number
    ticketNo: string
    coachRoute: ReservationListCoachRouteVM
    customer: SimpleEntity
    destination: ReservationListDestinationVM
    driver: ReservationListDriverVM
    pickupPoint: ReservationListPickupPointVM
    port: ReservationListPortVM
    portAlternate: ReservationListPortVM
    ship: ReservationListShipVM
    time: string
    passengerCount: number

}
