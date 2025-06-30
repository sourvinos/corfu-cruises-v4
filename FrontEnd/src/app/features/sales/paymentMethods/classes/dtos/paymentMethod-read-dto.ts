import { Metadata } from 'src/app/shared/classes/metadata'

export interface PaymentMethodReadDto extends Metadata {

    id: number
    description: string
    descriptionEn: string
    myDataId: string
    isCash: boolean
    isDefault: boolean
    isActive: boolean

}
