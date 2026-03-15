import { Injectable } from '@angular/core'
// Custom
import { ReservationImportDto } from '../dtos/reservation-import-dto'
import { ReservationImportListVM } from '../view-models/list/reservation-import-list-vm'

@Injectable({ providedIn: 'root' })

export class ReservationImportService {

    public buildReservations(x: ReservationImportListVM[]): ReservationImportDto[] {
        const i: ReservationImportDto[] = []
        x.forEach(z => {
            const user: ReservationImportDto = {
                reservationId: null,
                linkTwistId: z.code,
                date: z.date,
                customerId: z.customer.id,
                destinationId: z.destination.id,
                pickupPointId: z.pickupPoint.id,
                ticketNo: 'xxx',
                email: '',
                phones: '',
                adults: z.adults,
                kids: z.kids,
                free: z.free,
                remarks: '',
                passengers: [],
            }
            i.push(user)
        })
        return i
    }

}

