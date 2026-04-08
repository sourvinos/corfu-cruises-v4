import { Guid } from 'guid-typescript'

export interface PassengerImportDto {

    reservationId: Guid
    genderId: number
    nationalityId: number
    occupantId: number
    lastname: string
    firstname: string
    birthdate: string
    notes: string
    specialCare: string
    isBoarded: boolean

}