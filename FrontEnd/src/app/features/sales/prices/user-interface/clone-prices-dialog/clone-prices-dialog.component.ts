import { Component, Inject, NgZone } from '@angular/core'
import { FormGroup } from '@angular/forms'
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog'
// Custom
import { DexieService } from 'src/app/shared/services/dexie.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

@Component({
    selector: 'clone-prices-dialog',
    templateUrl: './clone-prices-dialog.component.html',
    styleUrls: ['./clone-prices-dialog.component.css']
})

export class ClonePricesDialogComponent {

    //#region variables

    private feature: string
    private table: string
    public form: FormGroup
    public records: SimpleEntity[]
    public selectedRecords: SimpleEntity[] = []
    private excludedRecord: SimpleEntity
    private customerIds = []

    //#endregion

    constructor(@Inject(MAT_DIALOG_DATA) public data: any, private dexieService: DexieService, private dialogRef: MatDialogRef<ClonePricesDialogComponent>, private messageLabelService: MessageLabelService, private ngZone: NgZone) {
        this.table = data[0]
        this.feature = data[1]
        this.excludedRecord = data[2]
    }

    //#region lifecycle hooks

    ngOnInit(): void {
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
        this.selectedRecords.forEach(record => {
            this.customerIds.push(record.id)
        })
        this.ngZone.run(() => {
            this.dialogRef.close(this.customerIds)
        })
    }

    //#endregion

    //#region private methods

    private populateDropdowns(): void {
        this.populateDropdownFromDexieDB(this.table, 'description')
    }

    private populateDropdownFromDexieDB(dexieTable: string, orderBy: string): void {
        this.dexieService.table(dexieTable).orderBy(orderBy).toArray().then((response) => {
            this.records = response.filter(x => x.description != this.excludedRecord.description)
        })
    }

    //#endregion

}
