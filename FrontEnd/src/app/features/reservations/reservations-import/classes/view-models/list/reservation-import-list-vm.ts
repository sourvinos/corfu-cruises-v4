import { PassengerImportDto } from '../../dtos/passenger-import-dto'
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
    pickupPoint: SimpleEntity
    details: PassengerImportDto[]
    status: SimpleEntity
    isValidPrimary: boolean
    isValidSecondary: boolean

}
