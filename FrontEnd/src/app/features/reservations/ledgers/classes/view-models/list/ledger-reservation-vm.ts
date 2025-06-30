import { LedgerDestinationVM } from './ledger-destination-vm'
import { LedgerPortVM } from './ledger-port-vm'
import { LedgerShipVM } from './ledger-ship-vm'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface LedgerReservationVM {

    date: string
    refNo: string
    reservationId: string
    destination: LedgerDestinationVM
    pickupPoint: SimpleEntity
    ship: LedgerShipVM
    port: LedgerPortVM
    ticketNo: string
    adults: number
    kids: number
    free: number
    totalPax: number
    embarkedPassengers: number
    totalNoShow: number
    remarks: string
    hasTransfer: boolean

}