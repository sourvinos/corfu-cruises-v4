import { Injectable } from '@angular/core'
// Custom
import { CheckInPassengerWriteDto } from '../dtos/check-in-passenger-write-dto'
import { CheckInReservationWriteDto } from '../dtos/check-in-reservation-write-dto'
import { DateHelperService } from 'src/app/shared/services/date-helper.service'
import { CheckInReservationReadDto } from '../dtos/check-in-reservation-read-dto'

@Injectable({ providedIn: 'root' })

export class CheckInHelperService {

    constructor(private dateHelperService: DateHelperService) { }

    //#region public methods

    public flattenForm(reservation: CheckInReservationReadDto): CheckInReservationWriteDto {
        const x: CheckInReservationWriteDto = {
            reservationId: reservation.reservationId,
            destinationId: reservation.destination.id,
            pickupPointId: reservation.pickupPoint.id,
            portId: 1,
            date: reservation.date,
            passengers: this.mapPassengers(reservation),
            putAt: reservation.putAt
        }
        return x
    }

    //#endregion

    //#region private methods

    private mapPassengers(reservation: CheckInReservationReadDto): CheckInPassengerWriteDto[] {
        const x = []
        reservation.passengers.forEach((passenger: any) => {
            const z: CheckInPassengerWriteDto = {
                reservationId: passenger.reservationId,
                genderId: passenger.gender.id,
                nationalityId: passenger.nationality.id,
                occupantId: 2,
                lastname: passenger.lastname,
                firstname: passenger.firstname,
                birthdate: this.dateHelperService.formatDateToIso(new Date(passenger.birthdate)),
                specialCare: passenger.specialCare,
                remarks: passenger.remarks
            }
            x.push(z)
        })
        return x
    }

    //#endregion

}
