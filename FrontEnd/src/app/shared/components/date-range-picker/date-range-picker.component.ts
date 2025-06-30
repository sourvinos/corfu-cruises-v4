import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms'
import { Component, EventEmitter, Input, Output } from '@angular/core'
import { DateAdapter } from '@angular/material/core'
// Custom
import { DateHelperService } from '../../services/date-helper.service'
import { InputTabStopDirective } from '../../directives/input-tabstop.directive'
import { InteractionService } from '../../services/interaction.service'
import { LocalStorageService } from '../../services/local-storage.service'
import { MatDatepickerInputEvent } from '@angular/material/datepicker'
import { MessageInputHintService } from '../../services/message-input-hint.service'
import { MessageLabelService } from '../../services/message-label.service'
import { ValidationService } from '../../services/validation.service'

@Component({
    selector: 'date-range-picker',
    templateUrl: './date-range-picker.component.html',
    styleUrls: ['./date-range-picker.component.css']
})

export class DateRangePickerComponent {

    //#region variables

    @Input() parentDateRange: string[]
    @Output() outputValues = new EventEmitter()

    public feature = 'date-range-picker'
    public form: FormGroup
    public input: InputTabStopDirective

    //#endregion

    constructor(private dateAdapter: DateAdapter<any>, private dateHelperService: DateHelperService, private formBuilder: FormBuilder, private interactionService: InteractionService, private localStorageService: LocalStorageService, private messageHintService: MessageInputHintService, private messageLabelService: MessageLabelService) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
        this.setLocale()
        this.subscribeToInteractionService()
    }

    //#endregion

    //#region public methods

    public doTodayTasks(): void {
        this.form.patchValue({
            fromDate: this.dateHelperService.formatDateToIso(new Date()),
            toDate: this.dateHelperService.formatDateToIso(new Date())
        })
        this.outputValues.emit(this.form)
    }

    public emitFormValues(event: MatDatepickerInputEvent<Date>): void {
        this.form.patchValue({
            date: new Date(event.value)
        })
        this.parentDateRange[0] = this.form.value.fromDate
        this.parentDateRange[1] = this.form.value.toDate
        this.outputValues.emit(this.form)
    }

    public getHint(id: string, minmax = 0): string {
        return this.messageHintService.getDescription(id, minmax)
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public isInvalidDateRange(): boolean {
        if (this.form.value.fromDate != null && this.form.value.toDate != null) {
            if (this.form.value.fromDate > this.form.value.toDate) {
                return true
            } else {
                this.dateHelperService.removeInvalidClassFromRangePicker()
            }
        } else {
            return true
        }
    }

    //#endregion

    //#region private methods

    private initForm(): void {
        this.form = this.formBuilder.group({
            fromDate: [new Date(this.parentDateRange[0]), Validators.required],
            toDate: [new Date(this.parentDateRange[1]), Validators.required]
        }, {
            validator: ValidationService.validDatePeriod
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

    //#region getters

    get fromDate(): AbstractControl {
        return this.form.get('fromDate')
    }

    get toDate(): AbstractControl {
        return this.form.get('toDate')
    }

    //#endregion

}

