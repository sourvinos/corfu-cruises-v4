import { BoardingReservationVM } from '../view-models/list/boarding-reservation-vm'

export class BoardingListResolved {

    constructor(public list: BoardingReservationVM, public error: any = null) { }

}
