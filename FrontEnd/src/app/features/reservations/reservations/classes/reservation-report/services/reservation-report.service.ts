import { Injectable } from '@angular/core'
// Custom
import { BooleanIconService } from 'src/app/shared/services/boolean-icon.service'
import { DateHelperService } from 'src/app/shared/services/date-helper.service'
import { HelperService } from 'src/app/shared/services/helper.service'
import { LogoService } from '../../../../reservations/classes/services/logo.service'
import { ReservationListVM } from '../../view-models/list/reservation-list-vm'
import { ReservationReportVM } from '../view-models/reservation-report-vm'
// Fonts
import pdfFonts from 'pdfmake/build/vfs_fonts'
import pdfMake from 'pdfmake/build/pdfmake'
import { strAkaAcidCanterBold } from '../../../../../../../assets/fonts/Aka-Acid-CanterBold.Base64.encoded'
import { strPFHandbookPro } from '../../../../../../../assets/fonts/PF-Handbook-Pro.Base64.encoded'

pdfMake.vfs = pdfFonts.pdfMake.vfs

@Injectable({ providedIn: 'root' })

export class ReservationReportPdfService {

    private reportVM: ReservationReportVM[]

    constructor(private booleanIconService: BooleanIconService, private dateHelperService: DateHelperService, private helperService: HelperService, private logoService: LogoService) { }

    //#region public methods

    public createReport(reservations: ReservationListVM[], date: string): void {
        this.flattedObjects(reservations)
        this.setFonts()
        const dd = {
            background: this.setBackgroundImage(),
            pageOrientation: 'landscape',
            pageSize: 'A4',
            content:
                [
                    {
                        table: {
                            body: [
                                [this.createTitle(date)],
                            ],
                            style: 'table',
                            widths: ['100%'],
                        },
                        layout: 'noBorders'
                    },
                    [
                        this.createTable(this.reportVM,
                            ['', '', '', '', '', '', '', '', '', '', '', '', '', '', '', ''],
                            ['refNo', 'ticketNo', 'customerDescription', 'destinationAbbreviation', 'coachRouteAbbreviation', 'pickupPointDescription', 'time', 'adults', 'kids', 'free', 'totalPax', 'totalPassengers', 'driverDescription', 'portAbbreviation', 'portAlternateAbbreviation', 'shipAbbreviation'],
                            ['center', 'left', 'left', 'center', 'center', 'left', 'center', 'right', 'right', 'right', 'right', 'right', 'left', 'center', 'center', 'center'])
                    ],
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
            },
            footer: (currentPage: { toString: () => string }, pageCount: string): void => {
                return this.createPageFooter(currentPage, pageCount)
            }
        }
        this.createPdf(dd)
    }

    //#endregion

    //#region private methods

    private createTitle(date: string): any {
        return {
            type: 'none',
            margin: [0, 0, 0, 0],
            ul: [
                { text: 'ΗΜΕΡΗΣΙΑ ΚΑΤΑΣΤΑΣΗ ΚΡΑΤΗΣΕΩΝ', fontSize: 13, style: 'AkaAcidCanterBold' },
                { text: this.dateHelperService.formatISODateToLocale(date, true, true), fontSize: 10, style: 'PFHandbookPro' }
            ]
        }
    }

    private createTableHeaders(): any[] {
        return [
            { text: 'RefNo', style: 'tableHeader', alignment: 'center', bold: false },
            { text: 'ΕΙΣΙΤΗΡΙΟ', style: 'tableHeader', alignment: 'left', bold: false },
            { text: 'ΠΕΛΑΤΗΣ', style: 'tableHeader', alignment: 'left', bold: false },
            { text: 'ΠΡΟΟΡΙΣΜΟΣ', style: 'tableHeader', alignment: 'center', bold: false },
            { text: 'ΔΙΑΔΡΟΜΗ', style: 'tableHeader', alignment: 'center', bold: false },
            { text: 'ΣΗΜΕΙΟ ΠΑΡΑΛΑΒΗΣ', style: 'tableHeader', alignment: 'left', bold: false },
            { text: 'ΩΡΑ', style: 'tableHeader', alignment: 'center', bold: false },
            { text: 'Ε', style: 'tableHeader', alignment: 'center', bold: false },
            { text: 'Π', style: 'tableHeader', alignment: 'center', bold: false },
            { text: 'Δ', style: 'tableHeader', alignment: 'center', bold: false },
            { text: 'Pax', style: 'tableHeader', alignment: 'center', bold: false },
            { text: 'Pax1', style: 'tableHeader', alignment: 'center', bold: false },
            { text: 'ΟΔΗΓΟΣ', style: 'tableHeader', alignment: 'center', bold: false },
            { text: 'ΛΙΜΑΝΙ Α', style: 'tableHeader', alignment: 'center', bold: false },
            { text: 'ΛΙΜΑΝΙ Β', style: 'tableHeader', alignment: 'center', bold: false },
            { text: 'ΠΛΟΙΟ', style: 'tableHeader', alignment: 'center', bold: false },
        ]
    }

    private createPageFooter(currentPage: { toString: any }, pageCount: string): any {
        return {
            layout: 'noBorders',
            margin: [0, 10, 40, 10],
            table: {
                widths: ['100%'],
                body: [
                    [
                        { text: 'ΣΕΛΙΔΑ ' + currentPage.toString() + ' ΑΠΟ ' + pageCount, alignment: 'right', fontSize: 6 }
                    ]
                ]
            }
        }
    }

    private flattedObjects(records: ReservationListVM[]): void {
        this.reportVM = []
        records.forEach(record => {
            this.reportVM.push({
                refNo: record.refNo,
                ticketNo: record.ticketNo,
                customerDescription: this.helperService.flattenObject(record.customer).description,
                destinationAbbreviation: this.helperService.flattenObject(record.destination).abbreviation,
                coachRouteAbbreviation: this.helperService.flattenObject(record.coachRoute).abbreviation,
                pickupPointDescription: this.helperService.flattenObject(record.pickupPoint).description,
                time: this.helperService.flattenObject(record.pickupPoint).time,
                adults: record.adults,
                kids: record.kids,
                free: record.free,
                totalPax: record.totalPax,
                totalPassengers: record.passengerCount,
                driverDescription: this.helperService.flattenObject(record.driver).description,
                portAbbreviation: this.helperService.flattenObject(record.port).abbreviation,
                portAlternateAbbreviation: this.helperService.flattenObject(record.portAlternate).abbreviation,
                shipAbbreviation: this.helperService.flattenObject(record.ship).abbreviation
            })
        })
    }

    private createPdf(document: any): void {
        pdfMake.createPdf(document).open()
    }

    private createTable(data: any[], columnTypes: any[], columns: any[], align: any[]): any {
        return {
            table: {
                headerRows: 1,
                dontBreakRows: true,
                body: this.createTableRows(data, columnTypes, columns, align),
                bold: false,
                style: 'table',
                layout: 'noBorders',
                widths: ['10%', '10%', '10%', '8%', '5%', '10%', '5%', '2%', '2%', '2%', '3%', '3%', '8%', '8%', '8%', '8%'],
                margin: [0, 10, 0, 0],
            },
            layout: 'lightHorizontalLines',
            margin: [0, 10, 0, 0]
        }
    }

    private createTableRows(data: any[], columnTypes: any[], columns: any[], align: any[]): void {
        const body: any = []
        body.push(this.createTableHeaders())
        data.forEach((row) => {
            let dataRow = []
            dataRow = this.processRow(columnTypes, columns, row, dataRow, align)
            body.push(dataRow)
        })
        return body
    }

    private processRow(columnTypes: any[], columns: any[], row: ReservationReportVM, dataRow: any[], align: any[]): any {
        columns.forEach((column, index) => {
            if (columnTypes[index] == 'boolean') {
                dataRow.push(row[column] == true
                    ? { image: this.booleanIconService.getTrueIcon(), fit: [8, 8], alignment: 'center' }
                    : { image: this.booleanIconService.getFalseIcon(), fit: [8, 8], alignment: 'center' })
            }
            if (columnTypes[index] == '') {
                dataRow.push({ text: this.formatField(columnTypes[index], row[column]), alignment: align[index].toString(), color: '#000000', noWrap: false, margin: [0, 1, 0, 0] })
            }
        })
        return dataRow
    }

    private formatField(type: any, field: string | number | Date): string {
        switch (type) {
            case 'date':
                return this.dateHelperService.formatISODateToLocale(field.toString(), false, true)
            default:
                return field != undefined ? field.toString() : ''
        }
    }

    private setBackgroundImage(): any[] {
        return [
            {
                image: this.logoService.getLogo('light'),
                width: '1000',
                opacity: 0.03
            }
        ]
    }

    private setFonts(): void {
        pdfFonts.pdfMake.vfs['AkaAcidCanterBold'] = strAkaAcidCanterBold
        pdfFonts.pdfMake.vfs['PFHandbookPro'] = strPFHandbookPro
        pdfMake.fonts = {
            AkaAcidCanterBold: { normal: 'AkaAcidCanterBold' },
            PFHandbookPro: { normal: 'PFHandbookPro' }
        }
    }

    //#endregion

}