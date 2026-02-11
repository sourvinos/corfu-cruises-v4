import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface ReservationImportListVM {

    code: string
    date: string
    destination: SimpleEntity
    customer: SimpleEntity
    adults: number
    kids: number
    free: number
    totalPax: number
    pickupPoint: string
    status: string
    isValidPrimary: boolean
    isValidSecondary: boolean

}
