import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms'
import { Component } from '@angular/core'
import { MatDatepickerInputEvent } from '@angular/material/datepicker'
import { Router } from '@angular/router'
// Custom
import { CheckInHttpService } from '../../classes/services/check-in.http.service'
import { DateHelperService } from 'src/app/shared/services/date-helper.service'
import { DialogService } from 'src/app/shared/services/modal-dialog.service'
import { LocalStorageService } from 'src/app/shared/services/local-storage.service'
import { MessageDialogService } from 'src/app/shared/services/message-dialog.service'
import { MessageInputHintService } from 'src/app/shared/services/message-input-hint.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'

@Component({
    selector: 'search',
    templateUrl: './search.component.html',
    styleUrls: ['./search.component.css', '../../assets/checkin.css']
})

export class SearchComponent {

    //#region variables

    public feature = 'check-in'
    public form: FormGroup
    public options: any[] = [{ 'id': 1, 'description': this.getLabel('step-2-yes') }, { 'id': 2, 'description': this.getLabel('step-2-no') }]
    public destinations: any[]
    public selectedDestination: string

    //#endregion

    constructor(private checkInService: CheckInHttpService, private dateHelperService: DateHelperService, private dialogService: DialogService, private formBuilder: FormBuilder, private localStorageService: LocalStorageService, private messageHintService: MessageInputHintService, private messageLabelService: MessageLabelService, private messageSnackbarService: MessageDialogService, private router: Router) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
        this.populateDropdowns()
        this.populateFieldsFromLocalStorage()
        this.highlightDropdownSelections()
    }

    //#endregion

    //#region public methods

    public doTodayTasks(): void {
        this.form.patchValue({
            complexGroup: {
                date: this.dateHelperService.formatDateToIso(new Date())
            }
        })
    }

    public getHint(id: string, minmax = 0): string {
        return this.messageHintService.getDescription(id, minmax)
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public onSearch(): void {
        if (this.form.value.hasRefNo == 1) {
            this.searchByRefNo().then((response) => {
                if (response) {
                    this.localStorageService.saveItem('criteria', JSON.stringify(this.form.value))
                    this.localStorageService.saveItem('reservation', JSON.stringify(response.body))
                    this.router.navigate(['checkIn/reservation'])
                }
            })
        }
        if (this.form.value.hasRefNo == 2) {
            this.searchByMultipleField().then((response) => {
                if (response) {
                    this.localStorageService.saveItem('criteria', JSON.stringify(this.form.value))
                    this.localStorageService.saveItem('reservation', JSON.stringify(response.body))
                    this.router.navigate(['checkIn/reservation'])
                }
            })
        }
    }

    public patchFormWithSelectedDate(event: MatDatepickerInputEvent<Date>): void {
        this.form.patchValue({
            complexGroup: {
                date: this.dateHelperService.formatDateToIso(new Date(event.value))
            }
        })
    }

    public requiredFieldsShouldBeGiven(): boolean {
        if (this.form.value.hasRefNo == '') {
            return true
        }
        if (this.form.value.hasRefNo == 1) {
            return this.form.value.refNo == ''
        }
        if (this.form.value.hasRefNo == 2) {
            return (this.form.value.complexGroup.date == '' || this.form.value.complexGroup.destination == '' || this.form.value.complexGroup.lastname == '' || this.form.value.complexGroup.firstname == '')
        }
    }

    //#endregion

    //#region private methods

    private getToday(): string {
        return (this.dateHelperService.formatDateToIso(new Date()))
    }

    private highlightDropdownSelections(): void {
        setTimeout(() => {
            this.selectedDestination = this.destinations.find(({ id }) => id == this.form.value.complexGroup.destination.id)
        }, 2000)
    }

    private initForm(): void {
        this.form = this.formBuilder.group({
            hasRefNo: '',
            refNo: ['', Validators.required],
            complexGroup: this.formBuilder.group({
                date: ['', Validators.required],
                destination: ['', Validators.required],
                lastname: ['', Validators.required],
                firstname: ['', Validators.required]
            })
        })
    }

    private populateDropdownFromLocalStorage(table: string): void {
        this[table] = JSON.parse(this.localStorageService.getItem(table))
    }

    private populateDropdowns(): void {
        this.populateDropdownFromLocalStorage('destinations')
    }

    private populateFieldsFromLocalStorage(): void {
        const x = this.localStorageService.getItem('criteria')
        if (x != '') {
            const criteria = JSON.parse(this.localStorageService.getItem('criteria'))
            this.form.setValue({
                hasRefNo: criteria.hasRefNo,
                refNo: criteria.refNo,
                complexGroup: {
                    date: criteria.complexGroup.date,
                    destination: {
                        id: criteria.complexGroup.destination.id,
                        description: criteria.complexGroup.destination.description
                    },
                    lastname: criteria.complexGroup.lastname,
                    firstname: criteria.complexGroup.firstname
                }
            })
        }
    }

    private searchByMultipleField(): Promise<any> {
        return new Promise((resolve) => {
            this.checkInService.getByDate(this.form.value.complexGroup.date, this.form.value.complexGroup.destination.id, this.form.value.complexGroup.lastname, this.form.value.complexGroup.firstname).subscribe({
                next: (response) => {
                    resolve(response)
                },
                error: (errorFromInterceptor) => {
                    this.showError(errorFromInterceptor)
                    resolve(false)
                }
            })
        })
    }

    private searchByRefNo(): Promise<any> {
        return new Promise((resolve) => {
            this.checkInService.getByRefNo(this.form.value.refNo).subscribe({
                next: (response) => {
                    resolve(response)
                },
                error: (errorFromInterceptor) => {
                    this.showError(errorFromInterceptor)
                    resolve(false)
                }
            })
        })
    }

    private showError(error: any): void {
        switch (error.status) {
            case 403:
                this.dialogService.open(this.messageSnackbarService.checkInAfterDepartureIsNotAllowed(), 'error', ['ok'])
                break
            case 404:
                this.dialogService.open(this.messageSnackbarService.reservationNotFound(), 'error', ['ok'])
                break
        }
    }

    //#endregion

    //#region getters

    get refNo(): AbstractControl {
        return this.form.get('refNo')
    }

    get complexGroup(): AbstractControl {
        return this.form.get('complexGroup')
    }

    get date(): AbstractControl {
        return this.form.get('complexGroup.date')
    }

    get destination(): AbstractControl {
        return this.form.get('complexGroup.destination')
    }

    get lastname(): AbstractControl {
        return this.form.get('complexGroup.lastname')
    }

    get firstname(): AbstractControl {
        return this.form.get('complexGroup.firstname')
    }

    //#endregion

}
