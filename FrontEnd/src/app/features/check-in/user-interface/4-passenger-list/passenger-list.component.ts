import { Component, Input, ViewChild } from '@angular/core'
import { MatDialog } from '@angular/material/dialog'
import { Router } from '@angular/router'
import { Table } from 'primeng/table'
// Custom
import { CheckInHelperService } from '../../classes/services/check-in.helper.service'
import { CheckInHttpService } from '../../classes/services/check-in.http.service'
import { CheckInPassengerReadDto } from '../../classes/dtos/check-in-passenger-read-dto'
import { DialogService } from 'src/app/shared/services/modal-dialog.service'
import { EmojiService } from 'src/app/shared/services/emoji.service'
import { HelperService } from 'src/app/shared/services/helper.service'
import { LocalStorageService } from 'src/app/shared/services/local-storage.service'
import { MessageDialogService } from 'src/app/shared/services/message-dialog.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { PassengerFormComponent } from '../5-passenger-form/passenger-form.component'
import { ReservationVM } from '../../classes/view-models/reservation-vm'

@Component({
    selector: 'passenger-list',
    templateUrl: './passenger-list.component.html',
    styleUrls: ['./passenger-list.component.css']
})

export class PassengerListComponent {

    //#region variables

    @Input() passengers: CheckInPassengerReadDto[] = []
    @ViewChild('table') table: Table | undefined
    public feature = 'check-in'
    public reservation: ReservationVM

    //#endregion

    constructor(private checkInHelperService: CheckInHelperService, private checkInHttpService: CheckInHttpService, private dialog: MatDialog, private dialogService: DialogService, private emojiService: EmojiService, private helperService: HelperService, private localStorageService: LocalStorageService, private messageDialogService: MessageDialogService, private messageLabelService: MessageLabelService, private router: Router) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.getReservationFromStorage()
    }

    //#endregion

    //#region public methods

    public checkTotalPaxAgainstPassengerCount(): boolean {
        return this.reservation.passengers.length < this.reservation.totalPax ? true : false
    }

    public deletePassenger(record: any): void {
        this.reservation.passengers.splice(this.reservation.passengers.indexOf(record), 1)
        this.localStorageService.saveItem('reservation', JSON.stringify(this.reservation))
    }

    public editPassenger(passenger: any): void {
        const dialog = this.dialog.open(PassengerFormComponent, {
            disableClose: true,
            width: '400px',
            data: {
                id: passenger.id,
                reservationId: passenger.reservationId,
                gender: { 'id': passenger.gender.id, 'description': passenger.gender.description },
                nationality: { 'id': passenger.nationality.id, 'description': passenger.nationality.description },
                lastname: passenger.lastname,
                firstname: passenger.firstname,
                birthdate: passenger.birthdate,
                remarks: passenger.remarks,
                specialCare: passenger.specialCare
            }
        })
        dialog.afterClosed().subscribe((result: any) => {
            if (result) {
                passenger = this.reservation.passengers.find(({ id }) => id === result.id)
                passenger.lastname = result.lastname
                passenger.firstname = result.firstname
                passenger.nationality = result.nationality
                passenger.birthdate = result.birthdate
                passenger.gender = result.gender
                passenger.specialCare = result.specialCare
                passenger.remarks = result.remarks
                this.localStorageService.saveItem('reservation', JSON.stringify(this.reservation))
            }
        })
    }

    public getEmoji(emoji: string): string {
        return this.emojiService.getEmoji(emoji)
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public highlightRow(id: any): void {
        this.helperService.highlightRow(id)
    }

    public next(): void {
        const x = this.checkInHelperService.flattenForm(JSON.parse(this.localStorageService.getItem('reservation')))
        this.checkInHttpService.updateReservation(x).subscribe({
            next: () => {
                this.router.navigate(['checkIn/email'])
            },
            error: (errorFromInterceptor) => {
                this.dialogService.open(this.messageDialogService.filterResponse(errorFromInterceptor), 'error', ['ok'])
            }
        })
    }

    public previous(): void {
        this.router.navigate(['checkIn/reservation'])
    }

    //#endregion

    //#region private methods

    private getReservationFromStorage(): void {
        this.reservation = JSON.parse(this.localStorageService.getItem('reservation'))
    }

    public showEmptyPassengerForm(): void {
        const dialog = this.dialog.open(PassengerFormComponent, {
            width: '400px',
            data: {
                id: 0,
                reservationId: this.reservation.reservationId,
                lastname: '',
                firstname: '',
                nationality: { 'id': 1, 'description': '' },
                gender: { 'id': 1, 'description': '' },
                birthdate: '',
                specialCare: '',
                remarks: ''
            }
        })
        dialog.afterClosed().subscribe((newPassenger: any) => {
            if (newPassenger) {
                this.reservation.passengers.push(newPassenger)
                this.localStorageService.saveItem('reservation', JSON.stringify(this.reservation))
            }
        })
    }

    //#endregion

}
