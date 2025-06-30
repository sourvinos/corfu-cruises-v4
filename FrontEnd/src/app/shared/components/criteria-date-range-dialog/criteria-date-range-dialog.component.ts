import { Component, Inject, NgZone } from '@angular/core'
import { DateAdapter } from '@angular/material/core'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog'
// Custom
import { DateHelperService } from 'src/app/shared/services/date-helper.service'
import { InteractionService } from 'src/app/shared/services/interaction.service'
import { LocalStorageService } from 'src/app/shared/services/local-storage.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { SessionStorageService } from 'src/app/shared/services/session-storage.service'
import { ValidationService } from '../../services/validation.service'

@Component({
    selector: 'criteria-date-range-dialog',
    templateUrl: './criteria-date-range-dialog.component.html',
    styleUrls: ['./criteria-date-range-dialog.component.css']
})

export class CriteriaDateRangeDialogComponent {

    //#region variables

    private feature: string
    public form: FormGroup

    //#endregion

    constructor(@Inject(MAT_DIALOG_DATA) public data: any, private dateAdapter: DateAdapter<any>, private dateHelperService: DateHelperService, private dialogRef: MatDialogRef<CriteriaDateRangeDialogComponent>, private formBuilder: FormBuilder, private interactionService: InteractionService, private localStorageService: LocalStorageService, private messageLabelService: MessageLabelService, private ngZone: NgZone, private sessionStorageService: SessionStorageService) {
        this.feature = data
    }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
        this.setLocale()
        this.populateFormFromStoredFields(this.getCriteriaFromStorage())
        this.subscribeToInteractionService()
    }

    //#endregion

    //#region public methods

    public close(): void {
        this.dialogRef.close()
    }

    public getDateRange(): any[] {
        const x = []
        x.push(this.form.value.fromDate)
        x.push(this.form.value.toDate)
        return x
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public patchFormWithSelectedDateRange(event: any): void {
        this.form.patchValue({
            fromDate: this.dateHelperService.formatDateToIso(new Date(event.value.fromDate)),
            toDate: this.dateHelperService.formatDateToIso(new Date(event.value.toDate))
        })
    }

    public onSearch(): void {
        this.ngZone.run(() => {
            this.interactionService.updateDateRange(this.form.value)
            this.sessionStorageService.saveItem(this.feature, JSON.stringify(this.form.value))
            this.dialogRef.close(this.form.value)
        })
    }

    //#endregion

    //#region private methods

    private getCriteriaFromStorage(): any {
        return this.sessionStorageService.getItem(this.feature)
            ? JSON.parse(this.sessionStorageService.getItem(this.feature)) 
            : ''
    }

    private initForm(): void {
        this.form = this.formBuilder.group({
            fromDate: ['', Validators.required],
            toDate: ['', Validators.required]
        }, {
            validator: ValidationService.validDatePeriod
        })
    }

    private populateFormFromStoredFields(object: any): void {
        this.form.patchValue({
            fromDate: object.fromDate,
            toDate: object.toDate
        })
    }

    private setLocale(): void {
        this.dateAdapter.setLocale(this.localStorageService.getLanguage())
    }

    private subscribeToInteractionService(): void {
        this.interactionService.refreshDateAdapter.subscribe(() => {
            this.setLocale()
        })
    }

    //#endregion

}
