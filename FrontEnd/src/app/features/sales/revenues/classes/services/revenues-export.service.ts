import FileSaver from 'file-saver'
import { Injectable } from '@angular/core'
// Custom
import { RevenuesExportVM } from '../view-models/export/revenues-export-vm'
import { RevenuesVM } from '../view-models/list/revenues-vm'

@Injectable({ providedIn: 'root' })

export class RevenuesExportService {

    //#region variables

    private exportVM: RevenuesExportVM[]

    //#endregion

    //#region public methods

    public buildVM(records: RevenuesVM[]): RevenuesExportVM[] {
        this.exportVM = []
        records.forEach(record => {
            this.exportVM.push({
                customer: record.customer.description,
                previous: record.previous,
                periodBalance: record.periodBalance,
                total: record.total
            })
        })
        return this.exportVM
    }

    public exportToExcel(x: RevenuesExportVM[], z: RevenuesExportVM[], i: RevenuesExportVM[]): void {
        import('xlsx').then((xlsx) => {
            const workbook = xlsx.utils.book_new()
            xlsx.utils.book_append_sheet(workbook, xlsx.utils.json_to_sheet(x), 'Corfu Cruises', true)
            xlsx.utils.book_append_sheet(workbook, xlsx.utils.json_to_sheet(z), 'Pandis Family', true)
            xlsx.utils.book_append_sheet(workbook, xlsx.utils.json_to_sheet(i), 'Total', true)
            const excelBuffer: any = xlsx.write(workbook, { bookType: 'xlsx', type: 'array' })
            this.saveAsExcelFile(excelBuffer, 'Revenues')
        })
    }

    //#endregion

    //#region private methods

    private saveAsExcelFile(buffer: any, fileName: string): void {
        const EXCEL_TYPE = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8'
        const EXCEL_EXTENSION = '.xlsx'
        const data: Blob = new Blob([buffer], { type: EXCEL_TYPE })
        FileSaver.saveAs(data, fileName + EXCEL_EXTENSION)
    }

    //#endregion

}
