import { Component } from '@angular/core'
import { MatDialog } from '@angular/material/dialog'
import { Router } from '@angular/router'
// Custom
import { LocalStorageService } from 'src/app/shared/services/local-storage.service'
import { ReservationSearchDialogComponent } from './reservation-search-dialog.component'
import { SessionStorageService } from 'src/app/shared/services/session-storage.service'

@Component({
    selector: 'reservation-search',
    templateUrl: './reservation-search.component.html',
    styleUrls: ['./reservation-search.component.css']
})

export class ReservationSearchComponent {

    constructor(private dialog: MatDialog, private localStorageService: LocalStorageService, private router: Router, private sessionStorageService: SessionStorageService) { }

    //#region public methods

    public getIconColor(): string {
        return this.localStorageService.getItem('theme') == 'light' ? 'black' : 'white'
    }

    public isLoggedIn(): boolean {
        return this.sessionStorageService.getItem('userId') ? true : false
    }

    public onOpenDialog(): void {
        const dialogRef = this.dialog.open(ReservationSearchDialogComponent, {
            data: ['search-reservation'],
            panelClass: 'dialog',
            height: '30rem',
            width: '32rem',
        })
        dialogRef.afterClosed().subscribe(result => {
            if (result !== undefined) {
                this.sessionStorageService.saveItem('isSearchByRefNo', 'true')
                this.router.navigate(['reservations/refNo', result.refNo])
            }
        })
    }

    //#endregion

}
