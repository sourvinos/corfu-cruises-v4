import { Injectable } from '@angular/core'
// Custom
import { SalesCriteriaVM } from '../view-models/form/sales-criteria-vm'
import { DateHelperService } from 'src/app/shared/services/date-helper.service'
import { DexieService } from 'src/app/shared/services/dexie.service'
import { DocumentTypeReadDto } from '../../../documentTypes/classes/dtos/documentType-read-dto'
import { InvoiceWriteDto } from '../dtos/form/invoice-write-dto'
import { PortWriteDto } from '../dtos/form/port-write-dto'

@Injectable({ providedIn: 'root' })

export class InvoiceHelperService {

    constructor(private dexieService: DexieService, private dateHelperService: DateHelperService) { }

    //#region public methods

    public validatePriceRetriever(x: SalesCriteriaVM): boolean {
        return x.date != 'NaN-NaN-NaN' && x.customerId != undefined && x.destinationId != undefined
    }

    public calculatePortA(formValue: any): any {
        const adults_A_AmountWithTransfer = formValue.portA.adults_A_WithTransfer * formValue.portA.adults_A_PriceWithTransfer
        const adults_A_AmountWithoutTransfer = formValue.portA.adults_A_WithoutTransfer * formValue.portA.adults_A_PriceWithoutTransfer
        const kids_A_AmountWithTransfer = formValue.portA.kids_A_WithTransfer * formValue.portA.kids_A_PriceWithTransfer
        const kids_A_AmountWithoutTransfer = formValue.portA.kids_A_WithoutTransfer * formValue.portA.kids_A_PriceWithoutTransfer
        const total_A_Persons = formValue.portA.adults_A_WithTransfer + formValue.portA.adults_A_WithoutTransfer + formValue.portA.kids_A_WithTransfer + formValue.portA.kids_A_WithoutTransfer + formValue.portA.free_A_WithTransfer + formValue.portA.free_A_WithoutTransfer
        const total_A_Amount = adults_A_AmountWithTransfer + adults_A_AmountWithoutTransfer + kids_A_AmountWithTransfer + kids_A_AmountWithoutTransfer
        return {
            adults_A_AmountWithTransfer,
            adults_A_AmountWithoutTransfer,
            kids_A_AmountWithTransfer,
            kids_A_AmountWithoutTransfer,
            total_A_Persons,
            total_A_Amount
        }
    }

    public calculatePortB(formValue: any): any {
        const adults_B_AmountWithTransfer = formValue.portB.adults_B_WithTransfer * formValue.portB.adults_B_PriceWithTransfer
        const adults_B_AmountWithoutTransfer = formValue.portB.adults_B_WithoutTransfer * formValue.portB.adults_B_PriceWithoutTransfer
        const kids_B_AmountWithTransfer = formValue.portB.kids_B_WithTransfer * formValue.portB.kids_B_PriceWithTransfer
        const kids_B_AmountWithoutTransfer = formValue.portB.kids_B_WithoutTransfer * formValue.portB.kids_B_PriceWithoutTransfer
        const total_B_Persons = formValue.portB.adults_B_WithTransfer + formValue.portB.adults_B_WithoutTransfer + formValue.portB.kids_B_WithTransfer + formValue.portB.kids_B_WithoutTransfer + formValue.portB.free_B_WithTransfer + formValue.portB.free_B_WithoutTransfer
        const total_B_Amount = adults_B_AmountWithTransfer + adults_B_AmountWithoutTransfer + kids_B_AmountWithTransfer + kids_B_AmountWithoutTransfer
        return {
            adults_B_AmountWithTransfer,
            adults_B_AmountWithoutTransfer,
            kids_B_AmountWithTransfer,
            kids_B_AmountWithoutTransfer,
            total_B_Persons,
            total_B_Amount
        }
    }

    public calculatePortTotals(formValue: any): any {
        const adultsWithTransfer = formValue.portA.adults_A_WithTransfer + formValue.portB.adults_B_WithTransfer
        const adultsAmountWithTransfer = formValue.portA.adults_A_AmountWithTransfer + formValue.portB.adults_B_AmountWithTransfer
        const adultsWithoutTransfer = formValue.portA.adults_A_WithoutTransfer + formValue.portB.adults_B_WithoutTransfer
        const adultsAmountWithoutTransfer = formValue.portA.adults_A_AmountWithoutTransfer + formValue.portB.adults_B_AmountWithoutTransfer
        const kidsWithTransfer = formValue.portA.kids_A_WithTransfer + formValue.portB.kids_B_WithTransfer
        const kidsAmountWithTransfer = formValue.portA.kids_A_AmountWithTransfer + formValue.portB.kids_B_AmountWithTransfer
        const kidsWithoutTransfer = formValue.portA.kids_A_WithoutTransfer + formValue.portB.kids_B_WithoutTransfer
        const kidsAmountWithoutTransfer = formValue.portA.kids_A_AmountWithoutTransfer + formValue.portB.kids_B_AmountWithoutTransfer
        const freeWithTransfer = formValue.portA.free_A_WithTransfer + formValue.portB.free_B_WithTransfer
        const freeWithoutTransfer = formValue.portA.free_A_WithoutTransfer + formValue.portB.free_B_WithoutTransfer
        const totalPersons = adultsWithTransfer + adultsWithoutTransfer + kidsWithTransfer + kidsWithoutTransfer + freeWithTransfer + freeWithoutTransfer
        const totalAmount =
            (formValue.portA.adults_A_WithTransfer * formValue.portA.adults_A_PriceWithTransfer) + (formValue.portB.adults_B_WithTransfer * formValue.portB.adults_B_PriceWithTransfer) +
            (formValue.portA.adults_A_WithoutTransfer * formValue.portA.adults_A_PriceWithoutTransfer) + (formValue.portB.adults_B_WithoutTransfer * formValue.portB.adults_B_PriceWithoutTransfer) +
            (formValue.portA.kids_A_WithTransfer * formValue.portA.kids_A_PriceWithTransfer) + (formValue.portB.kids_B_WithTransfer * formValue.portB.kids_B_PriceWithTransfer) +
            (formValue.portA.kids_A_WithoutTransfer * formValue.portA.kids_A_PriceWithoutTransfer) + (formValue.portB.kids_B_WithoutTransfer * formValue.portB.kids_B_PriceWithoutTransfer)
        return {
            adultsWithTransfer,
            adultsAmountWithTransfer,
            adultsWithoutTransfer,
            adultsAmountWithoutTransfer,
            kidsWithTransfer,
            kidsAmountWithTransfer,
            kidsWithoutTransfer,
            kidsAmountWithoutTransfer,
            freeWithTransfer,
            freeWithoutTransfer,
            totalPersons,
            totalAmount
        }
    }

    public calculateInvoiceSummary(formValue: any): any {
        const grossAmount = parseFloat(formValue.portTotals.total_Amount)
        const vatPercent = parseFloat(formValue.vatPercent) / 100
        const netAmount = grossAmount / (1 + vatPercent)
        const vatAmount = netAmount * vatPercent
        return {
            netAmount,
            vatPercent,
            vatAmount,
            grossAmount
        }
    }

    public flattenForm(formValue: any): InvoiceWriteDto {
        const x: InvoiceWriteDto = {
            invoiceId: formValue.invoiceId != '' ? formValue.invoiceId : null,
            customerId: formValue.customer.id,
            destinationId: formValue.destination.id,
            documentTypeId: formValue.documentType.id,
            paymentMethodId: formValue.paymentMethod.id,
            shipId: formValue.ship.id,
            date: this.dateHelperService.formatDateToIso(new Date(formValue.date)),
            tripDate: this.dateHelperService.formatDateToIso(new Date(formValue.tripDate)),
            invoiceNo: formValue.invoiceNo,
            netAmount: formValue.netAmount,
            vatPercent: formValue.vatPercent,
            vatAmount: formValue.vatAmount,
            grossAmount: formValue.grossAmount,
            remarks: formValue.remarks,
            putAt: formValue.putAt,
            invoicesPorts: this.mapPorts(formValue)
        }
        return x
    }

    public updateBrowserStorageAfterApiUpdate(record: DocumentTypeReadDto): void {
        this.dexieService.update('documentTypesInvoice', record)
    }

    //#endregion

    //#region private methods

    private mapPorts(formValue: any): PortWriteDto[] {
        const ports = []
        const x: PortWriteDto = {
            invoiceId: formValue.portA.invoiceId != '' ? formValue.portA.invoiceId : null,
            portId: formValue.portA.portId,
            adultsWithTransfer: formValue.portA.adults_A_WithTransfer,
            adultsPriceWithTransfer: formValue.portA.adults_A_PriceWithTransfer,
            adultsWithoutTransfer: formValue.portA.adults_A_WithoutTransfer,
            adultsPriceWithoutTransfer: formValue.portA.adults_A_PriceWithoutTransfer,
            kidsWithTransfer: formValue.portA.kids_A_WithTransfer,
            kidsPriceWithTransfer: formValue.portA.kids_A_PriceWithTransfer,
            kidsWithoutTransfer: formValue.portA.kids_A_WithoutTransfer,
            kidsPriceWithoutTransfer: formValue.portA.kids_A_PriceWithoutTransfer,
            freeWithTransfer: formValue.portA.free_A_WithTransfer,
            freeWithoutTransfer: formValue.portA.free_A_WithoutTransfer,
        }
        const z: PortWriteDto = {
            invoiceId: formValue.portB.invoiceId != '' ? formValue.portA.invoiceId : null,
            portId: formValue.portB.portId,
            adultsWithTransfer: formValue.portB.adults_B_WithTransfer,
            adultsPriceWithTransfer: formValue.portB.adults_B_PriceWithTransfer,
            adultsWithoutTransfer: formValue.portB.adults_B_WithoutTransfer,
            adultsPriceWithoutTransfer: formValue.portB.adults_B_PriceWithoutTransfer,
            kidsWithTransfer: formValue.portB.kids_B_WithTransfer,
            kidsPriceWithTransfer: formValue.portB.kids_B_PriceWithTransfer,
            kidsWithoutTransfer: formValue.portB.kids_B_WithoutTransfer,
            kidsPriceWithoutTransfer: formValue.portB.kids_B_PriceWithoutTransfer,
            freeWithTransfer: formValue.portB.free_B_WithTransfer,
            freeWithoutTransfer: formValue.portB.free_B_WithoutTransfer,
        }
        ports.push(x)
        ports.push(z)
        return ports
    }

    //#endregion

}
