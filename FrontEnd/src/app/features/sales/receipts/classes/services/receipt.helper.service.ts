import { Injectable } from '@angular/core'
// Custom
import { DateHelperService } from 'src/app/shared/services/date-helper.service'
import { DexieService } from 'src/app/shared/services/dexie.service'
import { DocumentTypeReadDto } from '../../../documentTypes/classes/dtos/documentType-read-dto'
import { ReceiptWriteDto } from '../dtos/receipt-write-dto'

@Injectable({ providedIn: 'root' })

export class ReceiptHelperService {

    constructor(private dexieService: DexieService, private dateHelperService: DateHelperService) { }

    public flattenForm(formValue: any): ReceiptWriteDto {
        const x: ReceiptWriteDto = {
            invoiceId: formValue.invoiceId != '' ? formValue.invoiceId : null,
            customerId: formValue.customer.id,
            documentTypeId: formValue.documentType.id,
            paymentMethodId: formValue.paymentMethod.id,
            shipOwnerId: formValue.shipOwner.id,
            date: this.dateHelperService.formatDateToIso(new Date(formValue.date)),
            tripDate: this.dateHelperService.formatDateToIso(new Date(formValue.tripDate)),
            invoiceNo: formValue.invoiceNo,
            grossAmount: formValue.grossAmount,
            remarks: formValue.remarks,
            isEmailSent: formValue.isEmailSent,
            isCancelled: formValue.isCancelled,
            putAt: formValue.putAt
        }
        return x
    }

    public updateBrowserStorageAfterApiUpdate(record: DocumentTypeReadDto): void {
        this.dexieService.update('documentTypesReceipt', record)
    }

}
