import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface ScheduleListVM {

    id: number
    date: string
    formattedDate: string
    year: SimpleEntity
    destination: SimpleEntity
    port: SimpleEntity
    maxPax: number

}
