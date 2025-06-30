import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms'
import { Component, NgZone } from '@angular/core'
import { MatDialogRef } from '@angular/material/dialog'
// Custom
import { MessageInputHintService } from '../../services/message-input-hint.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'

@Component({
    selector: 'reservation-search-dialog',
    templateUrl: './reservation-search-dialog.component.html',
    styleUrls: ['./reservation-search-dialog.component.css']
})

export class ReservationSearchDialogComponent {

    //#region variables

    private feature = 'search-reservation'
    public form: FormGroup

    //#endregion

    constructor(private dialogRef: MatDialogRef<ReservationSearchDialogComponent>, private formBuilder: FormBuilder, private messageHintService: MessageInputHintService, private messageLabelService: MessageLabelService, private ngZone: NgZone) {
        this.feature = 'search-reservation'
    }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
    }

    //#endregion

    //#region public methods

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public getHint(id: string, minmax = 0): string {
        return this.messageHintService.getDescription(id, minmax)
    }

    public onClose(): void {
        this.dialogRef.close()
    }

    public onSearch(): void {
        if (this.form.valid) {
            this.ngZone.run(() => {
                this.dialogRef.close(this.form.value)
            })
        }
    }

    //#endregion

    //#region private methods

    private initForm(): void {
        this.form = this.formBuilder.group({
            refNo: ['', Validators.required]
        })
    }

    //#endregion

    //#region getters

    get refNo(): AbstractControl {
        return this.form.get('refNo')
    }

    //#endregion

}
