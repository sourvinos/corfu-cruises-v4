import { Component, NgZone } from '@angular/core'
import { MatDialogRef } from '@angular/material/dialog'
// Custom
import { DateHelperService } from 'src/app/shared/services/date-helper.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'

@Component({
    selector: 'clone-reservation-dialog',
    templateUrl: './clone-reservation-dialog.component.html',
    styleUrls: ['./clone-reservation-dialog.component.css']
})

export class CloneReservationDialogComponent {

    //#region variables

    private feature = 'cloneReservationDialog'
    public daysSelected: any[]

    //#endregion

    constructor(
        private dateHelperService: DateHelperService,
        private dialogRef: MatDialogRef<CloneReservationDialogComponent>,
        private messageLabelService: MessageLabelService,
        private ngZone: NgZone
    ) { }

    ngOnInit(): void {
        this.initForm()
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public onSave(): void {
        this.ngZone.run(() => {
            this.dialogRef.close(this.daysSelected)
        })
    }

    private initForm(): void {
        this.daysSelected = []
    }

    public isSelected = (event: any): any => {
        const x = event.toDate()
        const date = this.dateHelperService.formatDateToIso(x)
        return this.daysSelected.find(x => x == date) ? 'selected' : null
    }

    public select(event: any, calendar: any): any {
        const x = event.toDate()
        const date = this.dateHelperService.formatDateToIso(x)
        const index = this.daysSelected.findIndex(x => x == date)
        if (index < 0) this.daysSelected.push(date)
        else this.daysSelected.splice(index, 1)
        calendar.updateTodaysDate()
    }

    public onClose(): void {
        this.dialogRef.close()
    }

}
