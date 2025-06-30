import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { ReservationParametersReadDto } from '../models/reservation-parameters-read.dto'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class ReservationParametersHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/reservationparameters')
    }

    //#region public methods

    public get(): Observable<ReservationParametersReadDto> {
        return this.http.get<ReservationParametersReadDto>(environment.apiUrl + '/reservationparameters')
    }

    //#endregion

}
