import { Metadata } from 'src/app/shared/classes/metadata'

export interface SaleParametersReadDto extends Metadata {

    id: number
    emailInvoicesIsActive: boolean

}
