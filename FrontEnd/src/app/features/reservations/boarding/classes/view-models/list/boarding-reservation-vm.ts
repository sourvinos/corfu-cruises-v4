import { BoardingDestinationVM } from './boarding-destination-vm'
import { BoardingPassengerVM } from './boarding-passenger-vm'
import { BoardingPortVM } from './boarding-port-vm'
import { BoardingShipVM } from './boarding-ship-vm'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface BoardingReservationVM {

    refNo: string
    ticketNo: string
    customer: SimpleEntity
    destination: BoardingDestinationVM
    pickupPoint: SimpleEntity
    driver: SimpleEntity
    port: BoardingPortVM
    ship: BoardingShipVM
    totalPax: number
    embarkedPassengers: number
    boardingStatus: boolean
    isBoarded: string
    remarks: string
    passengerIds: number[]

    passengers: BoardingPassengerVM[]

}
