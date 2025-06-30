import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { BoardingPassPassengerVM } from '../view-models/boarding-pass-passenger-vm'
import { BoardingPassVM } from '../view-models/boarding-pass-vm'
import { DateHelperService } from 'src/app/shared/services/date-helper.service'
import { HelperService } from 'src/app/shared/services/helper.service'
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { LogoService } from '../../services/logo.service'
import { ReservationParametersHttpService } from 'src/app/features/reservations/parameters/classes/services/reservation-parameters-http.service'
import { environment } from 'src/environments/environment'
// Fonts
import pdfMake from 'pdfmake/build/pdfmake'
import pdfFonts from 'pdfmake/build/vfs_fonts'
import { strPFHandbookPro } from '../../../../../../../assets/fonts/PF-Handbook-Pro.Base64.encoded'
import { strAkaAcidCanterBold } from '../../../../../../../assets/fonts/Aka-Acid-CanterBold.Base64.encoded'

pdfMake.vfs = pdfFonts.pdfMake.vfs

@Injectable({ providedIn: 'root' })

export class BoardingPassService extends HttpDataService {

    constructor(httpClient: HttpClient, private dateHelperService: DateHelperService, private helperService: HelperService, private logoService: LogoService, private reservationParametersHttpService: ReservationParametersHttpService) {
        super(httpClient, environment.apiUrl + '/reservations')
    }

    //#region public methods

    public createBoardingPass(form: any, companyPhones: string, companyEmail: string): BoardingPassVM {
        const boardingPass = {
            'date': this.dateHelperService.formatISODateToLocale(form.date),
            'refNo': form.refNo,
            'destinationDescription': form.destination.description,
            'customerDescription': form.customer.description,
            'pickupPointDescription': form.pickupPoint.description,
            'pickupPointExactPoint': form.exactPoint,
            'pickupPointTime': form.time,
            'adults': form.adults,
            'kids': form.kids,
            'free': form.free,
            'totalPax': form.totalPax,
            'driverDescription': form.driver.description,
            'ticketNo': form.ticketNo,
            'remarks': form.remarks,
            'barcode': form.refNo,
            'passengers': this.mapBoardingPassPassengers(form.passengers),
            'companyPhones': companyPhones,
            'companyEmail': companyEmail,
        }
        return boardingPass
    }

    public printBoardingPass(boardingPass: BoardingPassVM): void {
        this.setFonts()
        const rows = []
        rows.push([{ text: '' }, { text: '' }])
        rows.push([{ text: 'Passengers', colSpan: 2, alignment: 'center', fontSize: 18 }])
        for (const passenger of boardingPass.passengers) {
            rows.push([{ text: passenger.lastname, style: 'paddingLeft' }, { text: passenger.firstname }])
        }
        const dd = {
            pageMargins: [130, 30, 130, 250],
            pageOrientation: 'portrait',
            pageSize: 'A4',
            content: [
                {
                    table: {
                        body: [
                            [{ image: this.logoService.getLogo('light'), fit: [120, 120], alignment: 'center' }]
                        ],
                        widths: ['100%'],
                        heights: 130,
                    },
                    layout: 'noBorders'
                },
                {
                    table: {
                        body: [
                            [{ text: 'Your reservation for', alignment: 'center' }],
                            [{ text: boardingPass.destinationDescription, alignment: 'center', fontSize: 20, color: '#060770' }],
                            [{ text: 'is ready!', alignment: 'center' }]
                        ],
                        widths: ['100%'],
                        heights: [10, 20, 10],
                    },
                    layout: 'noBorders'
                },
                {
                    table: {
                        headerRows: 0,
                        body: [
                            [{ text: '' }, { text: '' }],
                            [{ text: 'Details', colSpan: 2, alignment: 'center', fontSize: 18 }],
                            [{ text: 'RefNo', style: 'paddingLeft' }, { text: boardingPass.refNo }],
                            [{ text: 'Ticket No', style: 'paddingLeft' }, { text: boardingPass.ticketNo }],
                            [{ text: 'Customer', style: 'paddingLeft' }, { text: boardingPass.customerDescription }],
                            [{ text: 'Remarks', style: 'paddingLeft' }, { text: boardingPass.remarks }],
                        ],
                        widths: ['50%', '50%'],
                        heights: [0, 15, 15, 15, 15],
                    },
                    layout: 'lightHorizontalLines'
                },
                {
                    table: {
                        headerRows: 0,
                        body: [
                            [{ text: '' }, { text: '' }],
                            [{ text: 'Pickup details', colSpan: 2, alignment: 'center', fontSize: 18, foreground: 'darkslategray' }],
                            [{ text: 'Date', style: 'paddingLeft' }, { text: boardingPass.date }],
                            [{ text: 'Pickup point', style: 'paddingLeft' }, { text: boardingPass.pickupPointDescription }],
                            [{ text: 'Exact point', style: 'paddingLeft' }, { text: boardingPass.pickupPointExactPoint }],
                            [{ text: 'Time', style: 'paddingLeft' }, { text: boardingPass.pickupPointTime }],
                        ],
                        widths: ['50%', '50%'],
                        heights: [0, 20, 15, 15, 15, 15],
                    },
                    layout: 'lightHorizontalLines'
                },
                {
                    table: {
                        headerRows: 0,
                        body: rows,
                        widths: ['50%', '50%']
                    },
                    layout: 'lightHorizontalLines'
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
                    margin: [50, 0, 0, 0]
                },
                paddingTop: {
                    margin: [0, 15, 0, 0]
                },
                small: {
                    fontSize: 8
                }
            },
            defaultStyle: {
                font: 'PFHandbookPro',
            },
            footer: {
                table: {
                    body: [
                        [{ qr: boardingPass.barcode, alignment: 'center', width: 200, style: ['paddingTop'], foreground: 'darkslategray' }],
                        [{ text: 'Problems? Questions? Please call', alignment: 'center', style: ['small', 'paddingTop'] }],
                        [{ text: boardingPass.companyPhones, alignment: 'center', style: 'small' }],
                        [{ text: 'or email at ' + boardingPass.companyEmail, alignment: 'center', style: 'small' }],
                        [{ text: '© Corfu Cruises 2023, Corfu - Greece', alignment: 'center', style: 'small' }],
                    ],
                    widths: ['100%'],
                },
                layout: 'noBorders'
            }
        }
        this.createPdf(dd)
    }

    public emailBoardingPass(reservationId: string): Observable<any> {
        return this.http.get<any>(this.url + '/boardingPass/' + reservationId)
    }

    //#endregion

    //#region private methods

    private createPdf(document: any): void {
        pdfMake.createPdf(document).open()
    }

    public getCompanyData(): Promise<any> {
        return new Promise((resolve) => {
            this.reservationParametersHttpService.get().subscribe((response) => {
                resolve(response)
            })
        })
    }

    private mapBoardingPassPassengers(passengers: any[]): BoardingPassPassengerVM[] {
        const x = []
        this.helperService.sortArray(passengers, 'lastname')
        passengers.forEach((element: any) => {
            const passenger = {
                'lastname': element.lastname,
                'firstname': element.firstname
            }
            x.push(passenger)
        })
        return x
    }

    private setFonts(): void {
        pdfFonts.pdfMake.vfs['AkaAcidCanterBold'] = strAkaAcidCanterBold
        pdfFonts.pdfMake.vfs['PFHandbookPro'] = strPFHandbookPro
        pdfMake.fonts = {
            PFHandbookPro: {
                normal: 'PFHandbookPro',
            },
            AkaAcidCanterBold: {
                normal: 'AkaAcidCanterBold'
            }
        }
    }

    //#endregion

}
