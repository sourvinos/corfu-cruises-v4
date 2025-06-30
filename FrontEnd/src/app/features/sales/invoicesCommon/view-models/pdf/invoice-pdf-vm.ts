import { InvoicePdfAadeVM } from './invoice-pdf-aade-vm'
import { InvoicePdfBankAccountVM } from './invoice-pdf-bankAccount-vm'
import { InvoicePdfHeaderVM } from './invoice-pdf-header-vm'
import { InvoicePdfPartyTypeVM } from './invoice-pdf-partyType-vm'
import { InvoicePdfPortVM } from './invoice-pdf-port-vm'
import { InvoicePdfShipVM } from './invoice-pdf-ship-vm'
import { InvoicePdfSummaryVM } from './invoice-pdf-summary-vm'

export interface InvoicePdfVM {

    header: InvoicePdfHeaderVM
    issuer: InvoicePdfPartyTypeVM
    customer: InvoicePdfPartyTypeVM
    aade: InvoicePdfAadeVM
    ports: InvoicePdfPortVM[]
    ship: InvoicePdfShipVM
    paymentMethod: string
    bankAccounts: InvoicePdfBankAccountVM[]
    summary: InvoicePdfSummaryVM
    previousBalance: number
    newBalance: number

}
