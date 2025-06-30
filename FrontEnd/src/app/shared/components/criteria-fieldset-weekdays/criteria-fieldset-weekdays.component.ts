import { Component, EventEmitter, Input, Output } from '@angular/core'
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms'
// Custom
import { EmojiService } from '../../services/emoji.service'
import { InteractionService } from '../../services/interaction.service'
import { MessageCalendarService } from '../../services/message-calendar.service'
import { MessageLabelService } from '../../services/message-label.service'
import { SimpleEntity } from '../../classes/simple-entity'
import { HelperService } from '../../services/helper.service'

@Component({
    selector: 'criteria-fieldset-weekdays',
    templateUrl: './criteria-fieldset-weekdays.component.html',
    styleUrls: ['criteria-fieldset-weekdays.component.css']
})

export class CriteriaFieldsetWeekdaysComponent {

    //#region variables

    @Input() caption: string
    @Input() feature: string
    @Input() selected: SimpleEntity[] = []
    @Output() outputSelected = new EventEmitter()

    public form: FormGroup
    public weekdays: SimpleEntity[] = []

    //#endregion

    constructor(private emojiService: EmojiService, private formBuilder: FormBuilder, private helperService: HelperService, private interactionService: InteractionService, private messageCalendarService: MessageCalendarService, private messageLabelService: MessageLabelService,) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
        this.populateWeekdays()
        this.subscribeToInteractionService()
    }

    //#endregion

    //#region public methods

    public getEmoji(emoji: string): string {
        return this.emojiService.getEmoji(emoji)
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public highlightRow(id: any): void {
        this.helperService.highlightRow(id)
    }

    public onHeaderCheckboxToggle(event: any, formControl: string): void {
        this.updateSelected(formControl)
        this.exportSelected()
    }

    public onRowSelect(event: any, formControl: string): void {
        this.updateSelected(formControl)
        this.exportSelected()
    }

    public onRowUnselect(event: any, formControl: string): void {
        this.updateSelected(formControl)
        this.exportSelected()
    }

    //#endregion

    //#region private methods

    private exportSelected(): void {
        this.outputSelected.emit(this.selected)
    }

    private initForm(): void {
        this.form = this.formBuilder.group({
            selected: this.formBuilder.array([], Validators.required)
        })
    }

    private populateWeekdays(): void {
        this.weekdays = [
            { id: 0, description: this.messageCalendarService.getDescription('weekdays', '0'), isActive: true },
            { id: 1, description: this.messageCalendarService.getDescription('weekdays', '1'), isActive: true },
            { id: 2, description: this.messageCalendarService.getDescription('weekdays', '2'), isActive: true },
            { id: 3, description: this.messageCalendarService.getDescription('weekdays', '3'), isActive: true },
            { id: 4, description: this.messageCalendarService.getDescription('weekdays', '4'), isActive: true },
            { id: 5, description: this.messageCalendarService.getDescription('weekdays', '5'), isActive: true },
            { id: 6, description: this.messageCalendarService.getDescription('weekdays', '6'), isActive: true }
        ]
    }

    private subscribeToInteractionService(): void {
        this.interactionService.refreshDateAdapter.subscribe(() => {
            this.populateWeekdays()
        })
    }

    private updateSelected(formControl: any): void {
        const x = this.form.controls[formControl] as FormArray
        this[formControl].forEach((element: any) => {
            x.push(new FormControl({
                'id': element.id,
                'description': element.description,
                'isActive': element.isActive
            }))
        })
    }

    //#endregion

}
