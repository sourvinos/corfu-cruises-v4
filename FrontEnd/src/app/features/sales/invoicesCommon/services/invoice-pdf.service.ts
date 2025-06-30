import { Injectable } from '@angular/core'
// Custom
import { InvoicePdfVM } from '../view-models/pdf/invoice-pdf-vm'
import { LogoService } from 'src/app/features/reservations/reservations/classes/services/logo.service'
// Fonts
import pdfFonts from 'pdfmake/build/vfs_fonts'
import pdfMake from 'pdfmake/build/pdfmake'
import { strAkaAcidCanterBold } from '../../../../../assets/fonts/Aka-Acid-CanterBold.Base64.encoded'
import { strPFHandbookPro } from '../../../../../assets/fonts/PF-Handbook-Pro.Base64.encoded'
import { InvoicePdfPortVM } from '../view-models/pdf/invoice-pdf-port-vm'

pdfMake.vfs = pdfFonts.pdfMake.vfs

@Injectable({ providedIn: 'root' })

export class InvoicePdfService {

    constructor(private logoService: LogoService) { }

    //#region public methods

    public async createPdf(invoice: InvoicePdfVM): Promise<void> {
        this.setFonts()
        const x = {
            pageOrientation: 'portrait',
            pageSize: 'A4',
            content:
                [
                    {
                        stack: [
                            {
                                table: {
                                    widths: ['*', 150],
                                    body: [
                                        [{ text: 'ΗΜΕΡΟΜΗΝΙΑ ΕΚΔΟΣΗΣ', alignment: 'right', borderColor: ['#ffffff', '#ffffff', '#efefef', '#ffffff'] }, { text: invoice.header.date, alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                        [{ text: 'ΠΑΡΑΣΤΑΤΙΚΟ', alignment: 'right', borderColor: ['#ffffff', '#ffffff', '#efefef', '#ffffff'] }, { text: invoice.header.documentType.description, alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                        [{ text: 'ΣΕΙΡΑ', alignment: 'right', borderColor: ['#ffffff', '#ffffff', '#efefef', '#ffffff'] }, { text: invoice.header.documentType.batch, alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                        [{ text: 'ΝΟ', alignment: 'right', borderColor: ['#ffffff', '#ffffff', '#efefef', '#ffffff'] }, { text: invoice.header.invoiceNo, alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                        [{ text: 'ΤΡΟΠΟΣ ΠΛΗΡΩΜΗΣ', alignment: 'right', borderColor: ['#ffffff', '#ffffff', '#efefef', '#ffffff'] }, { text: invoice.paymentMethod, alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }]
                                    ]
                                }
                            }
                        ],
                        margin: [0, 0, 0, 15]
                    },
                    {
                        stack: [
                            {
                                table: {
                                    widths: ['*', 150],
                                    body: [
                                        [{ text: 'ΗΜΕΡΟΜΗΝΙΑ ΕΚΔΡΟΜΗΣ', alignment: 'right', borderColor: ['#ffffff', '#ffffff', '#efefef', '#ffffff'] }, { text: invoice.header.tripDate, alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                        [{ text: 'ΠΛΟΙΟ', alignment: 'right', borderColor: ['#ffffff', '#ffffff', '#efefef', '#ffffff'] }, { text: invoice.ship.description + ' ' + invoice.ship.registryNo, alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }]
                                    ]
                                }
                            }
                        ],
                        margin: [0, 0, 0, 15]
                    },
                    {
                        stack: [
                            {
                                table: {
                                    widths: ['50%', '50%'],
                                    body: [[
                                        {
                                            table: {
                                                widths: [50, '*'],
                                                body: [
                                                    [{ text: 'ΤΑ ΣΤΟΙΧΕΙΑ ΜΑΣ', alignment: 'center', colSpan: 2, borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, ''],
                                                    [{ text: 'ΕΠΩΝΥΜΙΑ', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.issuer.fullDescription, borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                                    [{ text: 'ΔΡΑΣΤΗΡΙΟΤΗΤΑ', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.issuer.profession, borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                                    [{ text: 'ΑΦΜ', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.issuer.vatNumber, borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                                    [{ text: 'ΔΟΥ', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.issuer.taxOffice, borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                                    [{ text: 'ΟΔΟΣ', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.issuer.street, borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                                    [{ text: 'ΑΡΙΘΜΟΣ', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.issuer.number, borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                                    [{ text: 'ΠΟΛΗ', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.issuer.city, borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                                    [{ text: 'ΤΚ', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.issuer.postalCode, borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                                    [{ text: 'ΧΩΡΑ', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.issuer.country, borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                                    [{ text: 'ΤΗΛΕΦΩΝΑ', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.issuer.phones, borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                                    [{ text: 'EMAIL', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.issuer.email, borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                                ]
                                            },
                                        },
                                        {
                                            table: {
                                                widths: [50, '*'],
                                                body: [
                                                    [{ text: 'ΣΤΟΙΧΕΙΑ ΠΕΛΑΤΗ', alignment: 'center', colSpan: 2, borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, ''],
                                                    [{ text: 'ΕΠΩΝΥΜΙΑ', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.customer.fullDescription, borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                                    [{ text: 'ΔΡΑΣΤΗΡΙΟΤΗΤΑ', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.customer.profession, borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                                    [{ text: 'ΑΦΜ', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.customer.vatNumber, borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                                    [{ text: 'ΔΟΥ', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.customer.taxOffice, borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                                    [{ text: 'ΟΔΟΣ', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.customer.street, borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                                    [{ text: 'ΑΡΙΘΜΟΣ', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.customer.number, borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                                    [{ text: 'ΠΟΛΗ', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.customer.city, borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                                    [{ text: 'ΤΚ', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.customer.postalCode, borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                                    [{ text: 'ΧΩΡΑ', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.customer.country, borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                                    [{ text: 'ΤΗΛΕΦΩΝΑ', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.customer.phones, borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                                    [{ text: 'EMAIL', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.customer.email, borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                                ]
                                            },
                                        },
                                    ]]
                                },
                                layout: 'noBorders'
                            }],
                        margin: [0, 0, 0, 15]
                    },
                    {
                        table: {
                            widths: ['*', '*', '*'],
                            body: [
                                [
                                    {
                                        table: {
                                            widths: [50, '*', '*', '*'],
                                            body: [
                                                [{ text: 'ΑΝΑΧΩΡΗΣΕΙΣ ΑΠΟ CORFU PORT', colSpan: 4, borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, '', '', ''],
                                                [{ text: 'ΕΝΗΛΙΚΕΣ', colSpan: 4, borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, '', '', ''],
                                                [{ text: 'w/ TRANSFER', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.ports[0].adultsWithTransfer, alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.ports[0].adultsPriceWithTransfer.toFixed(2), alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.ports[0].adultsAmountWithTransfer.toFixed(2), alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                                [{ text: 'w/o TRANSFER', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.ports[0].adultsWithoutTransfer, alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.ports[0].adultsPriceWithoutTransfer.toFixed(2), alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.ports[0].adultsAmountWithoutTransfer.toFixed(2), alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                                [{ text: 'ΠΑΙΔΙΑ', colSpan: 4, borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, '', '', ''],
                                                [{ text: 'w/ TRANSFER', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.ports[0].kidsWithTransfer, alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.ports[0].kidsPriceWithTransfer.toFixed(2), alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.ports[0].kidsAmountWithTransfer.toFixed(2), alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                                [{ text: 'w/o TRANSFER', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.ports[0].kidsWithoutTransfer, alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.ports[0].kidsPriceWithoutTransfer.toFixed(2), alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.ports[0].kidsAmountWithoutTransfer.toFixed(2), alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                                [{ text: 'ΔΩΡΕΑΝ', colSpan: 4, borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, '', '', ''],
                                                [{ text: 'w/ TRANSFER', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.ports[0].freeWithTransfer, alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: '', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: '', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                                [{ text: 'w/o TRANSFER', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.ports[0].freeWithoutTransfer, alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: '', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: '', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                                [{ text: 'ΣΥΝΟΛΑ', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: this.calculateTotalPersonsPerPort(invoice.ports[0]), alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: '', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: this.calculateTotalAmountsPerPort(invoice.ports[0]), alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                            ],
                                        },
                                    },
                                    {
                                        table: {
                                            widths: [50, '*', '*', '*'],
                                            body: [
                                                [{ text: 'ΑΝΑΧΩΡΗΣΕΙΣ ΑΠΟ LEFKIMMI PORT', colSpan: 4, borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, '', '', ''],
                                                [{ text: 'ΕΝΗΛΙΚΕΣ', colSpan: 4, borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, '', '', ''],
                                                [{ text: 'w/ TRANSFER', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.ports[1].adultsWithTransfer, alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.ports[1].adultsPriceWithTransfer.toFixed(2), alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.ports[1].adultsAmountWithTransfer.toFixed(2), alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                                [{ text: 'w/o TRANSFER', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.ports[1].adultsWithoutTransfer, alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.ports[1].adultsPriceWithoutTransfer.toFixed(2), alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.ports[1].adultsAmountWithoutTransfer.toFixed(2), alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                                [{ text: 'ΠΑΙΔΙΑ', colSpan: 4, borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, '', '', ''],
                                                [{ text: 'w/ TRANSFER', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.ports[1].kidsWithTransfer, alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.ports[1].kidsPriceWithTransfer.toFixed(2), alignment: 'right' }, { text: invoice.ports[1].kidsAmountWithTransfer.toFixed(2), alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                                [{ text: 'w/o TRANSFER', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.ports[1].kidsWithoutTransfer, alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.ports[1].kidsPriceWithoutTransfer.toFixed(2), alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.ports[1].kidsAmountWithoutTransfer.toFixed(2), alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                                [{ text: 'ΔΩΡΕΑΝ', colSpan: 4, borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, '', '', ''],
                                                [{ text: 'w/ TRANSFER', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.ports[1].freeWithTransfer, alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: '', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: '', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                                [{ text: 'w/o TRANSFER', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.ports[1].freeWithoutTransfer, alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: '', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: '', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                                [{ text: 'ΣΥΝΟΛΑ', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: this.calculateTotalPersonsPerPort(invoice.ports[1]), alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: '', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: this.calculateTotalAmountsPerPort(invoice.ports[1]), alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                            ]
                                        },
                                    },
                                    {
                                        table: {
                                            widths: [50, '*', '*'],
                                            body: [
                                                [{ text: 'ΣΥΝΟΛΑ', colSpan: 3, borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, '', ''],
                                                [{ text: 'ΕΝΗΛΙΚΕΣ', colSpan: 3, borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, '', ''],
                                                [{ text: 'w/ TRANSFER', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.ports[0].adultsWithTransfer + invoice.ports[1].adultsWithTransfer, alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: (invoice.ports[0].adultsAmountWithTransfer + invoice.ports[1].adultsAmountWithTransfer).toFixed(2), alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                                [{ text: 'w/o TRANSFER', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.ports[0].adultsWithoutTransfer + invoice.ports[1].adultsWithoutTransfer, alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: (invoice.ports[0].adultsAmountWithoutTransfer + invoice.ports[1].adultsAmountWithoutTransfer).toFixed(2), alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                                [{ text: 'ΠΑΙΔΙΑ', colSpan: 3, borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, '', ''],
                                                [{ text: 'w/ TRANSFER', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.ports[0].kidsWithTransfer + invoice.ports[1].kidsWithTransfer, alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: (invoice.ports[0].kidsAmountWithTransfer + invoice.ports[1].kidsAmountWithTransfer).toFixed(2), alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                                [{ text: 'w/o TRANSFER', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.ports[0].kidsWithoutTransfer + invoice.ports[0].kidsWithoutTransfer, alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: (invoice.ports[0].kidsAmountWithoutTransfer + invoice.ports[1].kidsAmountWithoutTransfer).toFixed(2), alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                                [{ text: 'ΔΩΡΕΑΝ', colSpan: 3, borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, '', ''],
                                                [{ text: 'w/ TRANSFER', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.ports[0].freeWithTransfer + invoice.ports[1].freeWithTransfer, alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: '', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                                [{ text: 'w/o TRANSFER', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: invoice.ports[0].freeWithoutTransfer + invoice.ports[1].freeWithoutTransfer, alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: '', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                                [{ text: 'ΣΥΝΟΛΑ', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: this.calculateTotalPersons(invoice.ports), alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }, { text: this.calculateTotalAmounts(invoice.ports), alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }]
                                            ]
                                        }
                                    }
                                ]]
                        },
                        layout: 'noBorders'
                    },
                    {
                        stack: [
                            {
                                table: {
                                    widths: ['*', 150],
                                    body: [
                                        [{ text: 'ΚΑΘΑΡΗ ΑΞΙΑ', alignment: 'right', borderColor: ['#ffffff', '#ffffff', '#efefef', '#ffffff'] }, { text: invoice.summary.netAmount.toFixed(2), alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                        [{ text: 'ΦΠΑ 13%', alignment: 'right', borderColor: ['#ffffff', '#ffffff', '#efefef', '#ffffff'] }, { text: invoice.summary.vatAmount.toFixed(2), alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                        [{ text: 'ΣΥΝΟΛΙΚΗ ΑΞΙΑ', alignment: 'right', borderColor: ['#ffffff', '#ffffff', '#efefef', '#ffffff'] }, { text: invoice.summary.grossAmount.toFixed(2), alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                    ]
                                }
                            }
                        ],
                        margin: [0, 25, 0, 0],
                    },
                    {
                        stack: [
                            {
                                table: {
                                    widths: ['*', 150],
                                    body: [
                                        [{ text: 'ΠΡΟΗΓΟΥΜΕΝΟ ΥΠΟΛΟΙΠΟ', alignment: 'right', borderColor: ['#ffffff', '#ffffff', '#efefef', '#ffffff'] }, { text: invoice.previousBalance.toFixed(2), alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                        [{ text: 'ΝΕΟ ΥΠΟΛΟΙΠΟ', alignment: 'right', borderColor: ['#ffffff', '#ffffff', '#efefef', '#ffffff'] }, { text: invoice.newBalance.toFixed(2), alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }]
                                    ]
                                }
                            }
                        ],
                        margin: [0, 15, 0, 0],
                    },
                    {
                        stack: [
                            {
                                table: {
                                    widths: ['50%', '50%'],
                                    body: [[
                                        {
                                            table: {
                                                body: [
                                                    [{ text: 'ΤΡΑΠΕΖΙΚΟΙ ΛΟΓΑΡΙΑΣΜΟΙ', alignment: 'center', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'], margin: [0, 1, 0, 0] }],
                                                    [{ text: invoice.bankAccounts[0], alignment: 'left', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'], margin: [0, 1, 0, 0] }],
                                                    [{ text: invoice.bankAccounts[1], alignment: 'left', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'], margin: [0, 1, 0, 0] }],
                                                    [{ text: invoice.bankAccounts[2], alignment: 'left', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'], margin: [0, 1, 0, 0] }],
                                                    [{ text: invoice.bankAccounts[3], alignment: 'left', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'], margin: [0, 1, 0, 0] }],
                                                    [{ text: invoice.bankAccounts[4], alignment: 'left', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'], margin: [0, 1, 0, 0] }]
                                                ]
                                            },
                                        },
                                        {
                                            table: {
                                                widths: ['*', 150],
                                                body: [
                                                    [{ text: 'ΜΑΡΚ', alignment: 'right', borderColor: ['#ffffff', '#ffffff', '#efefef', '#ffffff'] }, { text: this.getMark(invoice.aade.mark), alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }],
                                                    [
                                                        {
                                                            text: '', borderColor: ['#ffffff', '#ffffff', '#efefef', '#ffffff']
                                                        },
                                                        {
                                                            qr: this.getQrCode(invoice.aade.url), fit: '50', alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'], foreground: this.setInvisible(invoice.aade.mark)
                                                        }
                                                    ],
                                                    [{ text: 'UID', alignment: 'right', borderColor: ['#ffffff', '#ffffff', '#efefef', '#ffffff'] }, { text: invoice.aade.uId, alignment: 'right', borderColor: ['#efefef', '#efefef', '#efefef', '#efefef'] }]
                                                ]
                                            },
                                        }
                                    ]]
                                }, layout: 'noBorders'
                            }
                        ],
                        margin: [0, 159, 0, 0]
                    },
                ],
            styles: {
                AkaAcidCanterBold: {
                    font: 'AkaAcidCanterBold',
                },
                PFHandbookPro: {
                    font: 'PFHandbookPro',
                },
                paddingLeft: {
                    margin: [40, 0, 0, 0]
                },
                paddingTop: {
                    margin: [0, 15, 0, 0]
                }
            },
            defaultStyle: {
                font: 'PFHandbookPro',
                fontSize: 7
            }
        }
        this.openPdf(x)
    }

    //#endregion

    //#region private methods

    private openPdf(document: any): void {
        pdfMake.createPdf(document).open()
    }

    private setFonts(): void {
        pdfFonts.pdfMake.vfs['AkaAcidCanterBold'] = strAkaAcidCanterBold
        pdfFonts.pdfMake.vfs['PFHandbookPro'] = strPFHandbookPro
        pdfMake.fonts = {
            AkaAcidCanterBold: { normal: 'AkaAcidCanterBold' },
            PFHandbookPro: { normal: 'PFHandbookPro' }
        }
    }

    private calculateTotalPersonsPerPort(port: InvoicePdfPortVM): number {
        return port.adultsWithTransfer + port.adultsWithoutTransfer + port.kidsWithTransfer + port.kidsWithoutTransfer + port.freeWithTransfer + port.freeWithoutTransfer
    }

    private calculateTotalAmountsPerPort(port: InvoicePdfPortVM): string {
        return (port.adultsAmountWithTransfer + port.adultsAmountWithoutTransfer + port.kidsAmountWithTransfer + port.kidsAmountWithoutTransfer).toFixed(2)
    }

    private calculateTotalPersons(ports: InvoicePdfPortVM[]): number {
        let x = 0
        ports.forEach(port => {
            x += port.adultsWithTransfer + port.adultsWithoutTransfer + port.kidsWithTransfer + port.kidsWithoutTransfer + port.freeWithTransfer + port.freeWithoutTransfer
        })
        return x
    }

    private calculateTotalAmounts(ports: InvoicePdfPortVM[]): string {
        let x = 0
        ports.forEach(port => {
            x += port.adultsAmountWithTransfer + port.adultsAmountWithoutTransfer + port.kidsAmountWithTransfer + port.kidsAmountWithoutTransfer
        })
        return x.toFixed(2)
    }

    private getQrCode(url: string): string {
        return url != '' ? url : 'https://mydataapidev.aade.gr/TimologioQR/QRInfo?q=CxLHW7Fo2Cx03w9SmhSh7zOw7duCFYONQpjJXJhJcNRhkaqaQNIEtBAeOQKZTx77lfl9PXd768EYGjl5eX%2fJvSWv4EmDrRaHHqvPO%2bRLCQM%3d'
    }

    private getMark(mark: string): string {
        return mark != '' ? mark : 'ΧΩΡΙΣ ΜΑΡΚ ΛΟΓΩ ΑΠΩΛΕΙΑΣ ΔΙΑΣΥΝΔΕΣΗΣ'
    }

    private setInvisible(mark: string): string {
        return mark != '' ? '#000000' : '#ffffff'
    }

    //#endregion

}
