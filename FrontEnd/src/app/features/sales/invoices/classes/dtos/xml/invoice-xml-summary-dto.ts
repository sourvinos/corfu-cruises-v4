import { InvoiceXmlIncomeClassificationDto } from './invoice-xml-incomeClassification-dto'

export interface InvoiceXmlSummaryDto {

    totalNetValue: number
    totalVatAmount: number,
    totalWithheldAmount: number
    totalFeesAmount: number
    totalStampDutyAmount: number
    totalOtherTaxesAmount: number
    totalDeductionsAmount: number
    totalGrossValue: number,
    incomeClassification: InvoiceXmlIncomeClassificationDto

}
