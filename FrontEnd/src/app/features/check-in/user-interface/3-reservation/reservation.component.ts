import { Component } from '@angular/core'
import { FormBuilder, FormGroup } from '@angular/forms'
import { Router } from '@angular/router'
import { LocalStorageService } from 'src/app/shared/services/local-storage.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { ReservationVM } from '../../classes/view-models/reservation-vm'
import { DateHelperService } from 'src/app/shared/services/date-helper.service'
// Custom

@Component({
    selector: 'reservation',
    templateUrl: './reservation.component.html',
    styleUrls: ['./reservation.component.css']
})

export class ReservationComponent {

    //#region variables

    public feature = 'check-in'
    public reservation: ReservationVM
    public reservationForm: FormGroup

    //#endregion

    constructor(private dateHelperService: DateHelperService, private formBuilder: FormBuilder, private localStorageService: LocalStorageService, private messageLabelService: MessageLabelService, private router: Router) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
        this.getRecord()
        this.populateFields()
    }

    //#endregion

    //#region public methods

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public next(): void {
        this.router.navigate(['checkIn/passenger-list'])
    }

    public previous(): void {
        this.router.navigate(['checkIn/search'])
    }

    //#endregion

    //#region private methods

    private getRecord(): void {
        this.reservation = JSON.parse(this.localStorageService.getItem('reservation')) as ReservationVM
    }

    private initForm(): void {
        this.reservationForm = this.formBuilder.group({
            date: '',
            refNo: '',
            ticketNo: '',
            destination: '',
            customer: '',
            pickupPoint: '',
            exactPoint: '',
            time: '',
            adults: '',
            kids: '',
            free: '',
            totalPax: '',
            phones: '',
            remarks: ''
        })
    }

    private populateFields(): void {
        this.reservationForm.setValue({
            date: this.dateHelperService.formatISODateToLocale(this.reservation.date, true, true),
            refNo: this.reservation.refNo,
            ticketNo: this.reservation.ticketNo,
            destination: this.reservation.destination.description,
            customer: this.reservation.customer.description,
            pickupPoint: this.reservation.pickupPoint.description,
            exactPoint: this.reservation.pickupPoint.exactPoint,
            time: this.reservation.pickupPoint.time,
            adults: this.reservation.adults,
            kids: this.reservation.kids,
            free: this.reservation.free,
            totalPax: this.reservation.totalPax,
            phones: this.reservation.phones,
            remarks: this.reservation.remarks
        })
    }

    //#endregion

}
