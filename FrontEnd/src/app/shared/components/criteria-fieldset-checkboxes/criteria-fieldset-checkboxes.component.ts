import { Component, EventEmitter, Input, Output } from '@angular/core'
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms'
// Custom
import { EmojiService } from '../../services/emoji.service'
import { HelperService } from '../../services/helper.service'
import { MessageLabelService } from '../../services/message-label.service'
import { SimpleEntity } from '../../classes/simple-entity'
import { SimpleCriteriaEntity } from '../../classes/simple-criteria-entity'

@Component({
    selector: 'criteria-fieldset-checkboxes',
    templateUrl: './criteria-fieldset-checkboxes.component.html',
    styleUrls: ['criteria-fieldset-checkboxes.component.css']
})

export class CriteriaFieldsetCheckboxesComponent {

    //#region variables

    @Input() array: SimpleCriteriaEntity[]
    @Input() caption: string
    @Input() feature: string
    @Input() selected: SimpleEntity[]
    @Output() outputSelected = new EventEmitter()

    public localSelected: SimpleCriteriaEntity[]
    public form: FormGroup

    //#endregion

    constructor(private emojiService: EmojiService, private formBuilder: FormBuilder, private helperService: HelperService, private messageLabelService: MessageLabelService) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
        this.updateLocalSelectedFromOuterSelected()
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
        this.exportLocalSelected()
    }

    public onRowSelect(): void {
        this.exportLocalSelected()
    }

    public onRowUnselect(): void {
        this.exportLocalSelected()
    }

    //#endregion

    //#region private methods

    private exportLocalSelected(): void {
        this.outputSelected.emit(this.localSelected)
    }

    private initForm(): void {
        this.form = this.formBuilder.group({
            selected: this.formBuilder.array([], Validators.required)
        })
    }

    private updateLocalSelectedFromOuterSelected(): void {
        this.localSelected = this.selected
    }

    private updateSelected(element: any): void {
        const x = this.form.controls['selected'] as FormArray
        x.push(new FormControl({
            'id': element.id,
            'description': element.description
        }))
    }

    //#endregion

}
