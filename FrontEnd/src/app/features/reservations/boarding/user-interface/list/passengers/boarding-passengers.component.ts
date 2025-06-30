import { Component, Inject, NgZone } from '@angular/core'
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog'
// Custom
import { BoardingHttpService } from '../../../classes/services/boarding.http.service'
import { BoardingPassengerVM } from '../../../classes/view-models/list/boarding-passenger-vm'
import { BoardingReservationVM } from '../../../classes/view-models/list/boarding-reservation-vm'
import { EmojiService } from './../../../../../../shared/services/emoji.service'
import { HelperService } from 'src/app/shared/services/helper.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'
import { environment } from 'src/environments/environment'

@Component({
    selector: 'boarding-passengers',
    templateUrl: './boarding-passengers.component.html',
    styleUrls: ['./boarding-passengers.component.css']
})

export class BoardingPassengerListComponent {

    //#region variables

    private feature = 'boardingList'
    public reservation: BoardingReservationVM
    public initialReservation: BoardingReservationVM

    public distinctNationalities: SimpleEntity[]

    //#endregion

    constructor(@Inject(MAT_DIALOG_DATA) public data: any, private dialogRef: MatDialogRef<BoardingPassengerListComponent>, private boardingService: BoardingHttpService, private emojiService: EmojiService, private helperService: HelperService, private messageLabelService: MessageLabelService, private ngZone: NgZone) {
        this.reservation = data.reservation
        this.initialReservation = JSON.parse(JSON.stringify(this.reservation))
        this.populateDropdownFilters()
        this.enableDisableFilters()
    }

    //#region public methods

    public close(): void {
        this.ngZone.run(() => {
            this.dialogRef.close(this.listMustBeRefreshed())
        })
    }

    public countMissingPassengers(): number {
        return this.reservation.totalPax - this.reservation.passengers.length
    }

    public doBoarding(ignoreCurrentStatus: boolean, passengers: BoardingPassengerVM[]): void {
        const ids: number[] = []
        passengers.forEach(passenger => {
            ids.push(passenger.id)
        })
        this.boardingService.embarkPassengers(ignoreCurrentStatus, ids).subscribe({
            complete: () => {
                passengers.forEach(passenger => {
                    const z = this.reservation.passengers.find(x => x.id == passenger.id)
                    z.isBoarded = ignoreCurrentStatus || !z.isBoarded
                })
            }
        })
    }

    public toggleBoardingStatus(passenger: BoardingPassengerVM): void {
        const passengers: BoardingPassengerVM[] = []
        passengers.push(passenger)
        this.doBoarding(false, passengers)
    }

    public getEmoji(emoji: string): string {
        return this.emojiService.getEmoji(emoji)
    }

    public getLabel(id: string, stringToReplace = ''): string {
        return this.messageLabelService.getDescription(this.feature, id, stringToReplace)
    }

    public getNationalityIcon(nationalityCode: string): any {
        return environment.nationalitiesIconDirectory + nationalityCode.toLowerCase() + '.png'
    }

    public isEmbarkAllAllowed(): boolean {
        return this.reservation.passengers.filter(x => x.isBoarded == false).length == 0
    }

    public isFilterDisabled(): boolean {
        return this.reservation.passengers.length == 0
    }

    public missingPassengers(): boolean {
        return this.reservation.totalPax != this.reservation.passengers.length
    }

    //#endregion

    //#region private methods

    private enableDisableFilters(): void {
        this.reservation.passengers.length == 0 ? this.helperService.disableTableFilters() : this.helperService.enableTableFilters()
    }

    private listMustBeRefreshed(): boolean {
        return !this.helperService.deepEqual(this.initialReservation.passengers, this.reservation.passengers)
    }

    private populateDropdownFilters(): void {
        if (this.reservation.passengers.length > 0) {
            this.distinctNationalities = this.helperService.getDistinctRecords(this.reservation.passengers, 'nationality', 'nationalityDescription')
        }
    }

    //#endregion

}
