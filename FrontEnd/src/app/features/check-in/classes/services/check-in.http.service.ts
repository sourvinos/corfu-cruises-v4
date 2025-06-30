import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { CheckInReservationWriteDto } from '../dtos/check-in-reservation-write-dto'
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { environment } from 'src/environments/environment'
import { ReservationVM } from '../view-models/reservation-vm'

@Injectable({ providedIn: 'root' })

export class CheckInHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/checkIn')
    }

    getByRefNo(refNo: string): Observable<ReservationVM> {
        return this.http.get<any>(this.url + '/refNo/' + refNo)
    }

    getByDate(date: string, destinationId: number, lastname: string, firstname: string): Observable<ReservationVM> {
        return this.http.get<any>(this.url + '/date/' + date + '/destinationId/' + destinationId + '/lastname/' + lastname + '/firstname/' + firstname)
    }

    updateReservation(reservation: CheckInReservationWriteDto): Observable<any> {
        return this.http.put<any>(this.url, reservation)
    }

    updateEmail(reservation: CheckInReservationWriteDto): Observable<any> {
        return this.http.patch<any>(this.url, reservation)
    }

    sendEmail(reservationId: string): Observable<any> {
        return this.http.get<any>(this.url + '/emailBoardingPass/' + reservationId)
    }

}
