import { Component, EventEmitter, Input, Output } from '@angular/core'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
// Custom
import { EmojiService } from '../../services/emoji.service'
import { HelperService } from '../../services/helper.service'
import { MessageLabelService } from '../../services/message-label.service'
import { SimpleEntity } from '../../classes/simple-entity'
import { SimpleCriteriaEntity } from '../../classes/simple-criteria-entity'

@Component({
    selector: 'criteria-fieldset-radios',
    templateUrl: './criteria-fieldset-radios.component.html',
    styleUrls: ['criteria-fieldset-radios.component.css']
})

export class CriteriaFieldsetRadiosComponent {

    //#region variables

    @Input() array: SimpleCriteriaEntity[]
    @Input() caption: string
    @Input() feature: string
    @Input() selected: SimpleEntity[]
    @Output() outputSelected = new EventEmitter()

    public localSelected: SimpleEntity[]
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

    public isSelected(): boolean {
        return typeof this.localSelected === 'object' && this.localSelected.length != 0
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
        this.outputSelected.emit(new Array(1).fill(this.localSelected))
    }

    private initForm(): void {
        this.form = this.formBuilder.group({
            selected: this.formBuilder.array([], Validators.required)
        })
    }

    private updateLocalSelectedFromOuterSelected(): void {
        this.localSelected = this.selected
    }

    //#endregion

}
