import FileSaver from 'file-saver'
import { Injectable } from '@angular/core'
// Custom
import { DateHelperService } from 'src/app/shared/services/date-helper.service'
import { ManifestCrewVM } from '../view-models/list/manifest-crew-vm'
import { ManifestExportCrewVM } from '../view-models/export/manifest-export-crew-vm'

@Injectable({ providedIn: 'root' })

export class ManifestExportCrewService {

    private exportCrew: ManifestExportCrewVM[]

    constructor(private dateHelperService: DateHelperService) { }

    public buildCrew(crew: ManifestCrewVM[]): ManifestExportCrewVM[] {
        let row = 0
        this.exportCrew = []
        crew.forEach(record => {
            this.exportCrew.push({
                Crew_Dep_Number: ++row,
                Crew_Dep_Family_name: record.lastname,
                Crew_Dep_Given_name: record.firstname,
                Crew_Dep_Gender: record.gender.description,
                Crew_Dep_Duty_of_crew: record.specialty.description,
                Crew_Dep_Nationality: record.nationality.code,
                Crew_Dep_Date_of_birth: this.dateHelperService.formatISODateToLocale(record.birthdate),
                Crew_Dep_Place_of_birth: '',
                Crew_Dep_Country_of_birth: null,
                Crew_Dep_Nature_of_identity_document: 'Other',
                Crew_Dep_Number_of_identity_document: '0',
                Crew_Dep_Issuing_State_of_Identity_Document: '',
                Crew_Dep_Expiry_Date_of_Identity_Document: '',
                Crew_Dep_Visa_Residence_Permit_number: ''
            })
        })
        return this.exportCrew
    }

    public exportToExcel(exportCrew: ManifestExportCrewVM[]): void {
        import('xlsx').then((xlsx) => {
            const worksheet = xlsx.utils.json_to_sheet(exportCrew)
            const workbook = { Sheets: { data: worksheet }, SheetNames: ['data'] }
            const excelBuffer: any = xlsx.write(workbook, { bookType: 'xlsx', type: 'array' })
            this.saveAsExcelFile(excelBuffer, 'Crew')
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
