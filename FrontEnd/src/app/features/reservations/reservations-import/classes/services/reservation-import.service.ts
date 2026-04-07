import { Injectable } from '@angular/core'
// Custom
import { PassengerImportDto } from '../dtos/passenger-import-dto'
import { ReservationImportDto } from '../dtos/reservation-import-dto'
import { ReservationImportListVM } from '../view-models/list/reservation-import-list-vm'

@Injectable({ providedIn: 'root' })

export class ReservationImportService {

    public buildReservations(x: ReservationImportListVM[]): ReservationImportDto[] {
        const reservations: ReservationImportDto[] = []
        x.forEach(reservation => {
            const z: ReservationImportDto = {
                reservationId: null,
                linkTwistId: reservation.code,
                date: reservation.date,
                customerId: reservation.customer.id,
                destinationId: reservation.destination.id,
                pickupPointId: reservation.pickupPoint.id,
                ticketNo: 'xxx',
                email: '',
                phones: '',
                adults: reservation.adults,
                kids: reservation.kids,
                free: reservation.free,
                remarks: '',
                passengers: this.buildPassengers(reservation.details),
            }
            reservations.push(z)
        })
        return reservations
    }

    private buildPassengers(passengers: PassengerImportDto[]): PassengerImportDto[] {
        return passengers
    }

}

