import { Component, Input } from '@angular/core'
import { FormBuilder, FormGroup } from '@angular/forms'
// Custom
import { MessageLabelService } from 'src/app/shared/services/message-label.service'

@Component({
    selector: 'aade-panel',
    templateUrl: './aade-panel.component.html',
    styleUrls: ['./aade-panel.component.css']
})

export class AadePanelComponent {

    //#region variables

    @Input() feature: string
    @Input() id: string
    @Input() uId: string
    @Input() mark: string
    @Input() markCancel: string
    @Input() authenticationCode: string
    @Input() iCode: string
    @Input() url: string
    @Input() discriminator: string

    public form: FormGroup

    //#endregion

    constructor(private formBuilder: FormBuilder, private messageLabelService: MessageLabelService) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
        this.populateFields()
    }

    ngOnChanges(): void {
        if (this.form != undefined)
            this.populateFields()
    }

    //#endregion

    //#region public methods

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    //#endregion

    //#region private methods

    private initForm(): void {
        this.form = this.formBuilder.group({
            id: '',
            uId: '',
            mark: '',
            markCancel: '',
            authenticationCode: '',
            iCode: '',
            url: '',
            discriminator: ''
        })
    }

    private populateFields(): void {
        this.form.setValue({
            id: this.id,
            uId: this.uId,
            mark: this.mark,
            markCancel: this.markCancel,
            authenticationCode: this.authenticationCode,
            iCode: this.iCode,
            url: this.url,
            discriminator: this.discriminator
        })
    }

    //#endregion

}
