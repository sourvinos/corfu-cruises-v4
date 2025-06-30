import { ActivatedRoute, Router } from '@angular/router'
import { Component } from '@angular/core'
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms'
import { MatAutocompleteTrigger } from '@angular/material/autocomplete'
import { Observable } from 'rxjs'
import { map, startWith } from 'rxjs/operators'
// Custom
import { CryptoService } from 'src/app/shared/services/crypto.service'
import { DexieService } from 'src/app/shared/services/dexie.service'
import { DialogService } from 'src/app/shared/services/modal-dialog.service'
import { EmojiService } from 'src/app/shared/services/emoji.service'
import { FormResolved } from 'src/app/shared/classes/form-resolved'
import { HelperService } from 'src/app/shared/services/helper.service'
import { InputTabStopDirective } from 'src/app/shared/directives/input-tabstop.directive'
import { MessageDialogService } from 'src/app/shared/services/message-dialog.service'
import { MessageInputHintService } from 'src/app/shared/services/message-input-hint.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { SessionStorageService } from 'src/app/shared/services/session-storage.service'
import { SimpleEntity } from './../../../../../shared/classes/simple-entity'
import { UpdateUserDto } from '../../classes/dtos/update-user-dto'
import { UserReadDto } from '../../classes/dtos/user-read-dto'
import { UserService } from '../../classes/services/user.service'
import { ValidationService } from '../../../../../shared/services/validation.service'

@Component({
    selector: 'edit-user-form',
    templateUrl: './edit-user-form.component.html',
    styleUrls: ['../../../../../../assets/styles/custom/forms.css', './edit-user-form.component.css']
})

export class EditUserFormComponent {

    //#region common #7

    private record: UserReadDto
    public feature = 'editUserForm'
    public featureIcon = 'users'
    public form: FormGroup
    public icon = ''
    public input: InputTabStopDirective
    public parentUrl = ''

    //#endregion

    //#region specific #2

    private mirrorRecord: UserReadDto
    private mustGoBackAfterSave = true

    //#endregion

    //#region autocompletes #2

    public isAutoCompleteDisabled = true
    public dropdownCustomers: Observable<SimpleEntity[]>

    //#endregion

    constructor(private activatedRoute: ActivatedRoute, private cryptoService: CryptoService, private dexieService: DexieService, private dialogService: DialogService, private emojiService: EmojiService, private formBuilder: FormBuilder, private helperService: HelperService, private messageDialogService: MessageDialogService, private messageHintService: MessageInputHintService, private messageLabelService: MessageLabelService, private router: Router, private sessionStorageService: SessionStorageService, private userService: UserService) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
        this.getRecord()
        this.populateFields()
        this.populateDropdowns()
        this.updateReturnUrl()
        this.cloneRecord()
        this.setSidebarsHeight()
    }

    ngAfterViewInit(): void {
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

    public isAdmin(): boolean {
        return this.cryptoService.decrypt(this.sessionStorageService.getItem('isAdmin')) == 'true' ? true : false
    }

    public onChangePassword(): void {
        if (this.cryptoService.decrypt(this.sessionStorageService.getItem('userId')) != this.record.id.toString()) {
            this.dialogService.open(this.messageDialogService.passwordCanBeChangedOnlyByAccountOwner(), 'error', ['ok'])
        } else {
            if (this.helperService.deepEqual(this.form.value, this.mirrorRecord)) {
                this.router.navigate(['/users/' + this.record.id.toString() + '/changePassword'])
            } else {
                this.mustGoBackAfterSave = false
                this.dialogService.open(this.messageDialogService.formIsDirty(), 'error', ['ok'])
            }
        }
    }

    public onDeleteDexieDB(): void {
        this.dialogService.open(this.messageDialogService.confirmDeleteDexieDB(), 'question', ['abort', 'ok']).subscribe(response => {
            if (response) {
                this.dexieService.delete().then(() => {
                    window.location.reload()
                })
            }
        })
    }

    public onSave(): void {
        this.saveRecord(this.flattenForm())
    }

    public onEmailUserDetais(): void {
        if (this.helperService.deepEqual(this.form.value, this.mirrorRecord)) {
            this.userService.patchUserWithNewEmailPending(this.form.value.id).subscribe({
                complete: () => {
                    this.helperService.doPostSaveFormTasks(this.messageDialogService.emailSent(), 'ok', this.parentUrl, true)
                },
                error: () => {
                    this.helperService.doPostSaveFormTasks(this.messageDialogService.emailNotSent(), 'error', this.parentUrl, true)
                }
            })
        } else {
            this.mustGoBackAfterSave = false
            this.dialogService.open(this.messageDialogService.formIsDirty(), 'error', ['ok'])
        }
    }

    public openOrCloseAutoComplete(trigger: MatAutocompleteTrigger, element: any): void {
        this.helperService.openOrCloseAutocomplete(this.form, element, trigger)
    }

    //#endregion

    //#region private methods

    private editUserFromList(): void {
        this.parentUrl = '/users'
        this.icon = 'arrow_back'
    }

    private editUserFromTopMenu(): void {
        this.parentUrl = '/home'
        this.icon = 'home'
    }

    private cloneRecord(): void {
        this.mirrorRecord = this.form.value
    }

    private filterAutocomplete(array: string, field: string, value: any): any[] {
        if (typeof value !== 'object') {
            const filtervalue = value.toLowerCase()
            return this[array].filter((element: { [x: string]: string }) =>
                element[field].toLowerCase().startsWith(filtervalue))
        }
    }

    private flattenForm(): UpdateUserDto {
        return {
            id: this.form.value.id,
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

    private getRecord(): Promise<any> {
        return new Promise((resolve) => {
            const formResolved: FormResolved = this.activatedRoute.snapshot.data['userEditForm']
            if (formResolved.error === null) {
                this.record = formResolved.record.body
                resolve(this.record)
            } else {
                this.dialogService.open(this.messageDialogService.filterResponse(formResolved.error), 'error', ['ok']).subscribe(() => {
                    this.resetForm()
                    this.goBack()
                })
            }
        })
    }

    private goBack(): void {
        this.router.navigate([this.parentUrl])
    }

    private initForm(): void {
        this.form = this.formBuilder.group({
            id: '',
            username: ['', [ValidationService.containsIllegalCharacters, Validators.maxLength(32), Validators.required]],
            displayname: ['', [ValidationService.beginsOrEndsWithSpace, Validators.maxLength(32), Validators.required]],
            customer: ['', [ValidationService.RequireAutocomplete]],
            email: ['', [Validators.email, Validators.maxLength(128), Validators.required]],
            isFirstFieldFocused: false,
            isAdmin: false,
            isActive: true,
            postAt: [''],
            postUser: [''],
            putAt: [''],
            putUser: ['']
        })
    }

    private populateDropdowns(): void {
        this.populateDropdownFromDexieDB('customers', 'dropdownCustomers', 'customer', 'description', 'description', true)
    }

    private populateDropdownFromDexieDB(dexieTable: string, filteredTable: string, formField: string, modelProperty: string, orderBy: string, includeWildCard: boolean): void {
        this.dexieService.table(dexieTable).orderBy(orderBy).toArray().then((response) => {
            this[dexieTable] = response
            includeWildCard ? this[dexieTable].unshift({ 'id': '0', 'description': '[' + this.emojiService.getEmoji('wildcard') + ']' }) : null
            this[filteredTable] = this.form.get(formField).valueChanges.pipe(startWith(''), map(value => this.filterAutocomplete(dexieTable, modelProperty, value)))
        })
    }

    private populateFields(): void {
        this.form.setValue({
            id: this.record.id,
            username: this.record.username,
            displayname: this.record.displayname,
            customer: { 'id': this.record.customer.id, 'description': this.record.customer.id == 0 ? this.emojiService.getEmoji('wildcard') : this.record.customer.description },
            email: this.record.email,
            isAdmin: this.record.isAdmin,
            isFirstFieldFocused: this.record.isFirstFieldFocused,
            isActive: this.record.isActive,
            postAt: this.record.postAt,
            postUser: this.record.postUser,
            putAt: this.record.putAt,
            putUser: this.record.putUser
        })
    }

    private resetForm(): void {
        this.form.reset()
    }

    private saveRecord(user: UpdateUserDto): void {
        this.userService.save(user).subscribe({
            complete: () => {
                this.sessionStorageService.saveItem('isFirstFieldFocused', user.isFirstFieldFocused.toString())
                this.mirrorRecord = this.form.value
                this.helperService.doPostSaveFormTasks(this.messageDialogService.success(), 'ok', this.parentUrl, this.mustGoBackAfterSave)
            },
            error: (errorFromInterceptor) => {
                this.dialogService.open(this.messageDialogService.filterResponse(errorFromInterceptor), 'error', ['ok'])
            }
        })
    }

    private setSidebarsHeight(): void {
        this.helperService.setSidebarsTopMargin('0')
    }

    private updateReturnUrl(): void {
        this.sessionStorageService.getItem('returnUrl') == '/' ? this.editUserFromTopMenu() : this.editUserFromList()
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

    get postAt(): AbstractControl {
        return this.form.get('postAt')
    }

    get postUser(): AbstractControl {
        return this.form.get('postUser')
    }

    get putAt(): AbstractControl {
        return this.form.get('putAt')
    }

    get putUser(): AbstractControl {
        return this.form.get('putUser')
    }

    //#endregion

}
