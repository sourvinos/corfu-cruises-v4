import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms'
import { Observable } from 'rxjs'
import { Component } from '@angular/core'
import { map, startWith } from 'rxjs/operators'
// Custom
import { ConfirmValidParentMatcher, ValidationService } from '../../../../../shared/services/validation.service'
import { DexieService } from 'src/app/shared/services/dexie.service'
import { DialogService } from 'src/app/shared/services/modal-dialog.service'
import { EmojiService } from 'src/app/shared/services/emoji.service'
import { HelperService } from 'src/app/shared/services/helper.service'
import { InputTabStopDirective } from 'src/app/shared/directives/input-tabstop.directive'
import { MatAutocompleteTrigger } from '@angular/material/autocomplete'
import { MessageDialogService } from 'src/app/shared/services/message-dialog.service'
import { MessageInputHintService } from 'src/app/shared/services/message-input-hint.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'
import { UserNewDto } from '../../classes/dtos/new-user-dto'
import { UserService } from '../../classes/services/user.service'

@Component({
    selector: 'new-user-form',
    templateUrl: './new-user-form.component.html',
    styleUrls: ['../../../../../../assets/styles/custom/forms.css']
})

export class NewUserFormComponent {

    //#region common #6

    public feature = 'newUserForm'
    public featureIcon = 'users'
    public form: FormGroup
    public icon = 'arrow_back'
    public input: InputTabStopDirective
    public parentUrl = '/users'

    //#endregion

    //#region specific #2

    public confirmValidParentMatcher = new ConfirmValidParentMatcher()
    public hidePassword = true

    //#endregion

    //#region autocompletes #2

    public isAutoCompleteDisabled = true
    public dropdownCustomers: Observable<SimpleEntity[]>

    //#endregion

    constructor(private dexieService: DexieService, private dialogService: DialogService, private emojiService: EmojiService, private formBuilder: FormBuilder, private helperService: HelperService, private messageDialogService: MessageDialogService, private messageHintService: MessageInputHintService, private messageLabelService: MessageLabelService, private userService: UserService) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
        this.populateDropdowns()
    }

    ngAfterViewInit(): void {
        this.clearInvisibleFieldsAndRestoreVisibility()
        this.focusOnField()
    }

    //#endregion

    //#region public methods

    public autocompleteFields(fieldName: any, object: any): any {
        return object ? object[fieldName] : undefined
    }

    public checkForEmptyAutoComplete(event: { target: { value: any } }): void {
        if (event.target.value == '') this.isAutoCompleteDisabled = true
    }

    public enableOrDisableAutoComplete(event: any): void {
        this.isAutoCompleteDisabled = this.helperService.enableOrDisableAutoComplete(event)
    }

    public getHint(id: string, minmax = 0): string {
        return this.messageHintService.getDescription(id, minmax)
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public onSave(): void {
        this.saveRecord(this.flattenForm())
    }

    public openOrCloseAutoComplete(trigger: MatAutocompleteTrigger, element: any): void {
        this.helperService.openOrCloseAutocomplete(this.form, element, trigger)
    }

    //#endregion

    //#region private methods

    private clearInvisibleFieldsAndRestoreVisibility(): void {
        this.helperService.clearInvisibleFieldsAndRestoreVisibility(this.form, ['email'])
    }

    private filterAutocomplete(array: string, field: string, value: any): any[] {
        if (typeof value !== 'object') {
            const filtervalue = value.toLowerCase()
            return this[array].filter((element: { [x: string]: string }) =>
                element[field].toLowerCase().startsWith(filtervalue))
        }
    }

    private flattenForm(): UserNewDto {
        return {
            username: this.form.value.username,
            displayname: this.form.value.displayname,
            customerId: this.form.value.customer.id == 0 ? null : this.form.value.customer.id,
            email: this.form.value.email,
            isFirstFieldFocused: this.form.value.isFirstFieldFocused,
            isAdmin: this.form.value.isAdmin,
            isActive: this.form.value.isActive
        }
    }

    private focusOnField(): void {
        this.helperService.focusOnField()
    }

    private initForm(): void {
        this.form = this.formBuilder.group({
            username: ['', [Validators.required, ValidationService.containsSpace, Validators.maxLength(256)]],
            displayname: ['', [Validators.required, ValidationService.beginsOrEndsWithSpace, Validators.maxLength(32)]],
            customer: ['', ValidationService.RequireAutocomplete],
            email: ['x@x.com', [Validators.email, Validators.maxLength(128), Validators.required]],
            isFirstFieldFocused: false,
            isAdmin: false,
            isActive: true
        })
    }

    private populateDropdowns(): void {
        this.populateDropdownFromDexieDB('customers', 'dropdownCustomers', 'customer', 'description', 'description', true)
    }

    private populateDropdownFromDexieDB(dexieTable: string, filteredTable: string, formField: string, modelProperty: string, orderBy: string, includeWildCard: boolean): void {
        this.dexieService.table(dexieTable).orderBy(orderBy).toArray().then((response) => {
            this[dexieTable] = response.filter(x => x.isActive)
            includeWildCard ? this[dexieTable].unshift({ 'id': '0', 'description': '[' + this.emojiService.getEmoji('wildcard') + ']' }) : null
            this[filteredTable] = this.form.get(formField).valueChanges.pipe(startWith(''), map(value => this.filterAutocomplete(dexieTable, modelProperty, value)))
        })
    }

    private saveRecord(user: UserNewDto): void {
        this.userService.saveUser(user).subscribe({
            complete: () => {
                this.helperService.doPostSaveFormTasks(this.messageDialogService.success(), 'ok', this.parentUrl, true)
            },
            error: (errorFromInterceptor) => {
                this.dialogService.open(this.messageDialogService.filterResponse(errorFromInterceptor), 'error', ['ok'])
            }
        })
    }

    //#endregion

    //#region getters

    get username(): AbstractControl {
        return this.form.get('username')
    }

    get displayname(): AbstractControl {
        return this.form.get('displayname')
    }

    get customer(): AbstractControl {
        return this.form.get('customer')
    }

    get email(): AbstractControl {
        return this.form.get('email')
    }

    get passwords(): AbstractControl {
        return this.form.get('passwords')
    }

    get password(): AbstractControl {
        return this.form.get('passwords.password')
    }

    get confirmPassword(): AbstractControl {
        return this.form.get('passwords.confirmPassword')
    }

    get matchingPasswords(): boolean {
        return this.form.get('passwords.password').value == this.form.get('passwords.confirmPassword').value
    }

    //#endregion

}
