import FileSaver from 'file-saver'
import { Injectable } from '@angular/core'
// Custom
import { BalanceSheetExportVM } from '../view-models/export/balanceSheet-export-vm'
import { BalanceSheetVM } from '../view-models/list/balanceSheet-vm'

@Injectable({ providedIn: 'root' })

export class BalanceSheetExportService {

    //#region variables

    private exportVM: BalanceSheetExportVM[]

    //#endregion

    //#region public methods

    public buildVM(records: BalanceSheetVM[]): BalanceSheetExportVM[] {
        this.exportVM = []
        records.forEach(record => {
            this.exportVM.push({
                customer: record.customer.description,
                previousBalance: record.previousBalance,
                debit: record.debit,
                credit: record.credit,
                actualBalance: record.actualBalance
            })
        })
        return this.exportVM
    }

    public exportToExcel(x: BalanceSheetExportVM[], z: BalanceSheetExportVM[], i: BalanceSheetExportVM[]): void {
        import('xlsx').then((xlsx) => {
            const _x = xlsx.utils.json_to_sheet(x)
            const _z = xlsx.utils.json_to_sheet(z)
            const _i = xlsx.utils.json_to_sheet(i)
            const workbook = xlsx.utils.book_new()
            xlsx.utils.book_append_sheet(workbook, _x, 'Corfu Cruises', true)
            xlsx.utils.book_append_sheet(workbook, _z, 'Pandis Family', true)
            xlsx.utils.book_append_sheet(workbook, _i, 'Total', true)
            const excelBuffer: any = xlsx.write(workbook, { bookType: 'xlsx', type: 'array' })
            this.saveAsExcelFile(excelBuffer, 'BalanceSheet')
        })
    }

    //#endregion

    //#region private methods

    private saveAsExcelFile(buffer: any, fileName: string): void {
        const EXCEL_TYPE = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8'
        const EXCEL_EXTENSION = '.xlsx'
        const data: Blob = new Blob([buffer], {
            type: EXCEL_TYPE
        })
        FileSaver.saveAs(data, fileName + EXCEL_EXTENSION)
    }

    //#endregion

}
