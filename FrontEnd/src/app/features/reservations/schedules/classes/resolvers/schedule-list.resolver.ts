import { Injectable } from '@angular/core'
import { Observable, of } from 'rxjs'
import { catchError, map } from 'rxjs/operators'
// Custom
import { ListResolved } from 'src/app/shared/classes/list-resolved'
import { ScheduleHttpService } from '../services/schedule-http.service'

@Injectable({ providedIn: 'root' })

export class ScheduleListResolver {

    constructor(private scheduleHttpService: ScheduleHttpService) { }

    resolve(): Observable<ListResolved> {
        return this.scheduleHttpService.getAll().pipe(
            map((scheduleList) => new ListResolved(scheduleList)),
            catchError((err: any) => of(new ListResolved(null, err)))
        )
    }

}
