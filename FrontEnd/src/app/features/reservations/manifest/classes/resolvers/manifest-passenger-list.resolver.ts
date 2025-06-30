import { Injectable } from '@angular/core'
import { Observable, of } from 'rxjs'
import { catchError, map } from 'rxjs/operators'
// Custom
import { ManifestPassengerListResolved } from './manifest-passenger-list-resolved'
import { ManifestSearchCriteriaVM } from '../view-models/criteria/manifest-search-criteria-vm'
import { ManifestHttpService } from '../services/manifest.http.service'
import { SessionStorageService } from 'src/app/shared/services/session-storage.service'

@Injectable({ providedIn: 'root' })

export class ManifestPassengerListResolver {

    constructor(private manifestService: ManifestHttpService, private sessionStorageService: SessionStorageService) { }

    resolve(): Observable<ManifestPassengerListResolved> {
        const storedCriteria = JSON.parse(this.sessionStorageService.getItem('manifest-criteria'))
        const searchCriteria: ManifestSearchCriteriaVM = {
            date: storedCriteria.date,
            destinationId: storedCriteria.selectedDestinations[0].id,
            portId: storedCriteria.selectedPorts[0].id,
            shipId: storedCriteria.selectedShips[0].id
        }
        return this.manifestService.getPassengers(searchCriteria).pipe(
            map((manifestList) => new ManifestPassengerListResolved(manifestList)),
            catchError((err: any) => of(new ManifestPassengerListResolved(null, err)))
        )
    }

}
