import { FormBuilder, FormGroup } from '@angular/forms'
import { Component, EventEmitter, Input, Output } from '@angular/core'
import { MatDatepickerInputEvent } from '@angular/material/datepicker'
// Custom
import { DateHelperService } from '../../services/date-helper.service'
import { MessageLabelService } from '../../services/message-label.service'

@Component({
    selector: 'date-picker-pill',
    templateUrl: './date-picker-pill.component.html',
    styleUrls: ['./date-picker-pill.component.css']
})

export class DatePickerPillComponent {

    //#region variables

    @Input() parentDate: string
    @Output() outputValue = new EventEmitter()

    public feature = 'date-picker-pill'
    public form: FormGroup

    //#endregion

    constructor(private dateHelperService: DateHelperService, private formBuilder: FormBuilder, private messageLabelService: MessageLabelService) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
    }

    //#endregion

    //#region public methods

    public doDateTasks(event: MatDatepickerInputEvent<Date>): void {
        this.form.patchValue({ date: this.dateHelperService.formatDateToIso(new Date(event.value)) })
        this.outputValue.emit(this.form.value.date)
    }

    public doTodayTasks(): void {
        this.form.patchValue({ date: this.dateHelperService.formatDateToIso(new Date()) })
        this.outputValue.emit(this.form.value.date)
    }

    public formatDateToLocale(date: string): string {
        return this.dateHelperService.formatISODateToLocale(this.dateHelperService.formatDateToIso(new Date(date)), true)
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    //#endregion

    //#region private methods

    private initForm(): void {
        this.form = this.formBuilder.group({
            date: [this.parentDate]
        })
    }

    //#endregion

}