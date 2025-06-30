import { Guid } from 'guid-typescript'

export interface UpdateUserDto {

    id: Guid
    username: string
    displayname: string
    customerId?: number
    email: string
    isFirstFieldFocused: boolean
    isAdmin: boolean
    isActive: boolean

}
