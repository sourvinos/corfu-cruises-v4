import { Component, NgZone } from '@angular/core'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { MatDialogRef } from '@angular/material/dialog'
// Custom
import { MessageLabelService } from 'src/app/shared/services/message-label.service'

@Component({
    selector: 'cached-reservation-dialog',
    templateUrl: './cached-reservation-dialog.component.html',
    styleUrls: ['./cached-reservation-dialog.component.css']
})

export class CachedReservationDialogComponent {

    //#region variables

    private feature = 'cachedReservationDialog'
    public form: FormGroup
    public options: any[]

    //#endregion

    constructor(private dialogRef: MatDialogRef<CachedReservationDialogComponent>, private formBuilder: FormBuilder, private messageLabelService: MessageLabelService, private ngZone: NgZone) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
        this.populateOptions()
    }

    //#endregion

    //#region public methods

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public save(): void {
        this.ngZone.run(() => {
            this.dialogRef.close(this.form.value)
        })
    }

    //#endregion

    //#region private methods

    private initForm(): void {
        this.form = this.formBuilder.group({
            selectedOption: ['', [Validators.required]]
        })
    }

    private populateOptions(): void {
        this.options = [
            { id: '1', description: 'Replace' },
            { id: '2', description: 'Delete' },
            { id: '3', description: 'Nothing' }
        ]
    }

    //#endregion

}
