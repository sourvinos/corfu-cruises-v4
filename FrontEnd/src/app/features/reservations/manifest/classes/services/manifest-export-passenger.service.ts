import FileSaver from 'file-saver'
import { Injectable } from '@angular/core'
// Custom
import { DateHelperService } from 'src/app/shared/services/date-helper.service'
import { ManifestExportPassengerVM } from '../view-models/export/manifest-export-passenger-vm'
import { ManifestPassengerVM } from '../view-models/list/manifest-passenger-vm'

@Injectable({ providedIn: 'root' })

export class ManifestExportPassengerService {

    private exportPassengers: ManifestExportPassengerVM[]

    constructor(private dateHelperService: DateHelperService) { }

    public buildPassengers(passengers: ManifestPassengerVM[]): ManifestExportPassengerVM[] {
        let row = 0
        this.exportPassengers = []
        passengers.forEach(record => {
            this.exportPassengers.push({
                Passengers_Dep_Number: ++row,
                Passengers_Dep_Family_name: record.lastname,
                Passengers_Dep_Given_name: record.firstname,
                Passengers_Dep_Gender: record.gender.description,
                Passengers_Dep_Nationality: record.nationality.code,
                Passengers_Dep_Date_of_birth: this.dateHelperService.formatISODateToLocale(record.birthdate),
                Passengers_Dep_Place_of_birth: '',
                Passengers_Dep_Country_of_birth: null,
                Passengers_Dep_Nature_of_identity_document: 'Other',
                Passengers_Dep_Number_of_identity_document: '0',
                Passengers_Dep_Issuing_State_of_Identity_Document: '',
                Passengers_Dep_Expiry_Date_of_Identity_Document: '',
                Passengers_Dep_Port_of_embarkation: record.port.locode,
                Passengers_Dep_Port_of_disembarkation: null,
                Passengers_Dep_Transit: null,
                Passengers_Dep_Visa_Residence_Permit_number: '',
                Passengers_Dep_Special_Care_Or_Assistance: '',
                Passengers_Dep_Emergency_Contact_Number: ''
            })
        })
        return this.exportPassengers
    }

    public exportToExcel(exportPassengers: ManifestExportPassengerVM[]): void {
        import('xlsx').then((xlsx) => {
            const worksheet = xlsx.utils.json_to_sheet(exportPassengers)
            const workbook = { Sheets: { data: worksheet }, SheetNames: ['data'] }
            const excelBuffer: any = xlsx.write(workbook, { bookType: 'xlsx', type: 'array' })
            this.saveAsExcelFile(excelBuffer, 'Passengers')
        })
    }

    private saveAsExcelFile(buffer: any, fileName: string): void {
        const EXCEL_TYPE = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8'
        const EXCEL_EXTENSION = '.xlsx'
        const data: Blob = new Blob([buffer], {
            type: EXCEL_TYPE
        })
        FileSaver.saveAs(data, fileName + EXCEL_EXTENSION)
    }

}
