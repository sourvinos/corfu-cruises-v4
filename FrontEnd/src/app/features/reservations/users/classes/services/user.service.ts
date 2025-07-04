import { HttpClient, HttpParams } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { ChangePasswordViewModel } from '../view-models/change-password-view-model'
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class UserService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/users')
    }

    //#region public methods

    public updatePassword(formData: ChangePasswordViewModel): Observable<any> {
        return this.http.post<any>(this.url + '/changePassword/', formData)
    }

    public saveUser(formData: any): Observable<any> {
        return this.http.post<any>(this.url, formData)
    }

    public patchUserWithNewEmailPending(id: string): Observable<any> {
        const params = new HttpParams().set('id', id)
        return this.http.patch<any>(this.url + '/patchUserWithNewEmailPending?', null, { params: params })
    }

    //#endregion

}
