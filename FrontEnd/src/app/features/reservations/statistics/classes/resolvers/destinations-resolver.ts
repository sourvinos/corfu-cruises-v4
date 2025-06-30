import { Injectable } from '@angular/core'
import { Observable, of } from 'rxjs'
import { catchError, map } from 'rxjs/operators'
// Custom
import { ListResolved } from 'src/app/shared/classes/list-resolved'
import { SessionStorageService } from 'src/app/shared/services/session-storage.service'
import { StatisticsService } from '../services/statistics.service'

@Injectable({ providedIn: 'root' })

export class DestinationsResolver {

    constructor(private sessionStorageService: SessionStorageService, private statisticsService: StatisticsService) { }

    resolve(): Observable<ListResolved> {
        return this.statisticsService.getStatistics(this.sessionStorageService.getItem('selectedYear'), 'destinations').pipe(
            map((statistics) => new ListResolved(statistics)),
            catchError((err: any) => of(new ListResolved(null, err)))
        )
    }

}
