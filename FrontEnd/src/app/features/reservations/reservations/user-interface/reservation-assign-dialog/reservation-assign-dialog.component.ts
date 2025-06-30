import { Component, Inject, NgZone } from '@angular/core'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog'
// Custom
import { DexieService } from 'src/app/shared/services/dexie.service'
import { LocalStorageService } from 'src/app/shared/services/local-storage.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

@Component({
    selector: 'reservation-assign-dialog',
    templateUrl: './reservation-assign-dialog.component.html',
    styleUrls: ['./reservation-assign-dialog.component.css']
})

export class ReservationAssignDialogComponent {

    //#region variables

    private feature: string
    private table: string
    public form: FormGroup
    public records: SimpleEntity[]

    //#endregion

    constructor(@Inject(MAT_DIALOG_DATA) public data: any, private dexieService: DexieService, private dialogRef: MatDialogRef<ReservationAssignDialogComponent>, private formBuilder: FormBuilder, private localStorageService: LocalStorageService, private messageLabelService: MessageLabelService, private ngZone: NgZone) {
        this.table = data[0]
        this.feature = data[1]
    }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
        this.populateDropdowns()
    }

    //#endregion

    //#region public methods

    public close(): void {
        this.dialogRef.close()
    }

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
            record: ['', Validators.required]
        })
    }

    private populateDropdowns(): void {
        this.populateDropdownFromDexieDB(this.table, 'description')
    }

    private populateDropdownFromDexieDB(dexieTable: string, orderBy: string): void {
        this.dexieService.table(dexieTable).orderBy(orderBy).toArray().then((response) => {
            this.records = response.filter(x => x.isActive)
        })
    }

    //#endregion

}
