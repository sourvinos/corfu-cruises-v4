import { InvoiceXmlIncomeClassificationDto } from './invoice-xml-incomeClassification-dto'

export interface InvoiceXmlRowDto {

    lineNumber: number
    netValue: number
    vatCategory: number
    vatAmount: number,
    incomeClassification: InvoiceXmlIncomeClassificationDto

}
