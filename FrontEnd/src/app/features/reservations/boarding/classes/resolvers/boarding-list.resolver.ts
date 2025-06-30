import { Injectable } from '@angular/core'
import { Observable, of } from 'rxjs'
import { catchError, map } from 'rxjs/operators'
// Custom
import { BoardingHttpService } from '../services/boarding.http.service'
import { BoardingListResolved } from './boarding-list-resolved'
import { BoardingSearchCriteriaVM } from '../view-models/criteria/boarding-search-criteria-vm'
import { SessionStorageService } from 'src/app/shared/services/session-storage.service'

@Injectable({ providedIn: 'root' })

export class BoardingListResolver {

    constructor(private boardingService: BoardingHttpService, private sessionStorageService: SessionStorageService) { }

    resolve(): Observable<BoardingListResolved> {
        const storedCriteria = JSON.parse(this.sessionStorageService.getItem('boarding-criteria'))
        const searchCriteria: BoardingSearchCriteriaVM = {
            date: storedCriteria.date,
            destinationIds: this.buildIds(storedCriteria.selectedDestinations),
            portIds: this.buildIds(storedCriteria.selectedPorts),
            shipIds: this.buildIds(storedCriteria.selectedShips)
        }
        return this.boardingService.get(searchCriteria).pipe(
            map((ledgerList) => new BoardingListResolved(ledgerList)),
            catchError((err: any) => of(new BoardingListResolved(null, err)))
        )
    }

    private buildIds(criteria: any): number[] {
        const ids = []
        criteria.forEach((element: { id: any }) => {
            ids.push(parseInt(element.id))
        })
        return ids
    }

}
