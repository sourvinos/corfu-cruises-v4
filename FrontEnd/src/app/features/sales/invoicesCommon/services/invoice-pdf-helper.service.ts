import { Injectable } from '@angular/core'
// Custom
import { DateHelperService } from 'src/app/shared/services/date-helper.service'
import { InvoicePdfAadeVM } from '../view-models/pdf/invoice-pdf-aade-vm'
import { InvoicePdfHeaderVM } from '../view-models/pdf/invoice-pdf-header-vm'
import { InvoicePdfPartyTypeVM } from '../view-models/pdf/invoice-pdf-partyType-vm'
import { InvoicePdfPortVM } from '../view-models/pdf/invoice-pdf-port-vm'
import { InvoicePdfShipVM } from '../view-models/pdf/invoice-pdf-ship-vm'
import { InvoicePdfSummaryVM } from '../view-models/pdf/invoice-pdf-summary-vm'
import { InvoicePdfVM } from '../view-models/pdf/invoice-pdf-vm'

@Injectable({ providedIn: 'root' })

export class InvoicePdfHelperService {

    constructor(private dateHelperService: DateHelperService) { }

    //#region public methods

    public async createPdfInvoiceParts(invoice: InvoicePdfVM): Promise<any> {
        const header = await this.buildHeader(invoice)
        const issuer = await this.buildIssuer(invoice.issuer)
        const customer = await this.buildCounterPart(invoice.customer)
        const summary = await this.buildSummary(invoice)
        const aade = await this.buildAade(invoice.aade)
        const ports = await this.buildPorts(invoice.ports)
        const ship = await this.buildShip(invoice.ship)
        const paymentMethod = await this.buildPaymentMethod(invoice.paymentMethod)
        const bankAccounts = await this.buildBankAccounts()
        const previousBalance = invoice.previousBalance
        const newBalance = invoice.newBalance
        return {
            header,
            issuer,
            customer,
            summary,
            aade,
            ports,
            ship,
            paymentMethod,
            bankAccounts,
            previousBalance,
            newBalance
        }
    }

    //#endregion

    //#region private methods

    private buildHeader(invoice: InvoicePdfVM): Promise<any> {
        return new Promise((resolve) => {
            const x: InvoicePdfHeaderVM = {
                date: this.dateHelperService.formatISODateToLocale(invoice.header.date),
                tripDate: this.dateHelperService.formatISODateToLocale(invoice.header.tripDate),
                documentType: {
                    description: invoice.header.documentType.description,
                    batch: invoice.header.documentType.batch,
                },
                invoiceNo: invoice.header.invoiceNo
            }
            resolve(x)
        })
    }

    private buildIssuer(issuer: InvoicePdfPartyTypeVM): Promise<any> {
        return new Promise((resolve) => {
            const x: InvoicePdfPartyTypeVM = {
                branch: issuer.branch,
                city: issuer.city,
                country: issuer.country,
                email: issuer.email,
                fullDescription: issuer.fullDescription,
                number: issuer.number,
                phones: issuer.phones,
                postalCode: issuer.postalCode,
                profession: issuer.profession,
                street: issuer.street,
                taxOffice: issuer.taxOffice,
                vatNumber: issuer.vatNumber
            }
            resolve(x)
        })
    }

    private buildCounterPart(counterPart: InvoicePdfPartyTypeVM): Promise<any> {
        return new Promise((resolve) => {
            const x: InvoicePdfPartyTypeVM = {
                branch: counterPart.branch,
                city: counterPart.city,
                country: counterPart.country,
                email: counterPart.email,
                fullDescription: counterPart.fullDescription,
                number: counterPart.number,
                phones: counterPart.phones,
                postalCode: counterPart.postalCode,
                profession: counterPart.profession,
                street: counterPart.street,
                taxOffice: counterPart.taxOffice,
                vatNumber: counterPart.vatNumber
            }
            resolve(x)
        })
    }

    private buildSummary(invoice: InvoicePdfVM): Promise<any> {
        return new Promise((resolve) => {
            const x: InvoicePdfSummaryVM = {
                netAmount: invoice.summary.netAmount,
                vatPercent: invoice.summary.vatPercent,
                vatAmount: invoice.summary.vatAmount,
                grossAmount: invoice.summary.grossAmount
            }
            resolve(x)
        })

    }

    private buildAade(aade: InvoicePdfAadeVM): Promise<any> {
        return new Promise((resolve) => {
            const x: InvoicePdfAadeVM = {
                id: aade.id,
                uId: aade.uId,
                mark: aade.mark,
                markCancel: aade.markCancel,
                authenticationCode: aade.authenticationCode,
                iCode: aade.iCode,
                url: aade.url,
            }
            resolve(x)
        })
    }

    private buildPorts(ports: InvoicePdfPortVM[]): Promise<any> {
        const x = []
        return new Promise((resolve) => {
            const z: InvoicePdfPortVM = {
                adultsWithTransfer: ports[0].adultsWithTransfer,
                adultsPriceWithTransfer: ports[0].adultsPriceWithTransfer,
                adultsAmountWithTransfer: ports[0].adultsWithTransfer * ports[0].adultsPriceWithTransfer,
                adultsWithoutTransfer: ports[0].adultsWithoutTransfer,
                adultsPriceWithoutTransfer: ports[0].adultsPriceWithoutTransfer,
                adultsAmountWithoutTransfer: ports[0].adultsWithoutTransfer * ports[0].adultsPriceWithoutTransfer,
                kidsWithTransfer: ports[0].kidsWithTransfer,
                kidsPriceWithTransfer: ports[0].kidsPriceWithTransfer,
                kidsAmountWithTransfer: ports[0].kidsWithTransfer * ports[0].kidsPriceWithTransfer,
                kidsWithoutTransfer: ports[0].kidsWithoutTransfer,
                kidsPriceWithoutTransfer: ports[0].kidsPriceWithoutTransfer,
                kidsAmountWithoutTransfer: ports[0].kidsWithoutTransfer * ports[0].kidsPriceWithoutTransfer,
                freeWithTransfer: ports[0].freeWithTransfer,
                freeWithoutTransfer: ports[0].freeWithoutTransfer
            }
            x.push(z)
            const i: InvoicePdfPortVM = {
                adultsWithTransfer: ports[1].adultsWithTransfer,
                adultsPriceWithTransfer: ports[1].adultsPriceWithTransfer,
                adultsAmountWithTransfer: ports[1].adultsWithTransfer * ports[1].adultsPriceWithTransfer,
                adultsWithoutTransfer: ports[1].adultsWithoutTransfer,
                adultsPriceWithoutTransfer: ports[1].adultsPriceWithoutTransfer,
                adultsAmountWithoutTransfer: ports[1].adultsWithoutTransfer * ports[1].adultsPriceWithoutTransfer,
                kidsWithTransfer: ports[1].kidsWithTransfer,
                kidsPriceWithTransfer: ports[1].kidsPriceWithTransfer,
                kidsAmountWithTransfer: ports[1].kidsWithTransfer * ports[1].kidsPriceWithTransfer,
                kidsWithoutTransfer: ports[1].kidsWithoutTransfer,
                kidsPriceWithoutTransfer: ports[1].kidsPriceWithoutTransfer,
                kidsAmountWithoutTransfer: ports[1].kidsWithoutTransfer * ports[1].kidsPriceWithoutTransfer,
                freeWithTransfer: ports[1].freeWithTransfer,
                freeWithoutTransfer: ports[1].freeWithoutTransfer
            }
            x.push(i)
            resolve(x)
        })
    }

    private buildShip(ship: InvoicePdfShipVM): Promise<any> {
        return new Promise((resolve) => {
            const x: InvoicePdfShipVM = {
                description: ship.description,
                registryNo: ship.registryNo
            }
            resolve(x)
        })
    }

    private buildPaymentMethod(paymentMethod: string): Promise<any> {
        return new Promise((resolve) => {
            const x: string = paymentMethod
            resolve(x)
        })
    }

    private buildBalances(): Promise<any> {
        return new Promise((resolve) => {
            const x: number[] = [
                1234.56,
                7890.56
            ]
            resolve(x)
        })
    }

    private buildBankAccounts(): Promise<any> {
        return new Promise((resolve) => {
            const x: string[] = [
                'ΠΕΙΡΑΙΩΣ GR17 0171 1740 0061 7413 5517 925',
                'ALPHA BANK GR41 0140 5950 5950 0233 0002 010',
                'EUROBANK GR53 0260 4450 0003 5020 0621 503',
                'ΕΘΝΙΚΗ GR22 0110 8670 0000 8670 0263 444',
                'ATTICA BANK GR43 0160 8730 0000 0008 5207 750',
                ' '
            ]
            resolve(x)
        })

    }

    //#endregion

}
