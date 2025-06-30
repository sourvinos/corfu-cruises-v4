import { ActivatedRouteSnapshot } from '@angular/router'
import { Injectable } from '@angular/core'
// Custom
import { FormResolved } from 'src/app/shared/classes/form-resolved'
import { ScheduleHttpService } from '../services/schedule-http.service'
import { catchError, map, of } from 'rxjs'

@Injectable({ providedIn: 'root' })

export class ScheduleEditFormResolver {

    constructor(private schedulehttpService: ScheduleHttpService) { }

    resolve(route: ActivatedRouteSnapshot): any {
        return this.schedulehttpService.getSingle(route.params.id).pipe(
            map((scheduleEditForm) => new FormResolved(scheduleEditForm)),
            catchError((err: any) => of(new FormResolved(null, err)))
        )
    }

}
