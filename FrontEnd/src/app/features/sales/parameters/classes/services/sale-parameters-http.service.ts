import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { SaleParametersReadDto } from '../models/sale-parameters-read.dto'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class SaleParametersHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/saleparameters')
    }

    //#region public methods

    public get(): Observable<SaleParametersReadDto> {
        return this.http.get<SaleParametersReadDto>(environment.apiUrl + '/saleparameters')
    }

    //#endregion

}
