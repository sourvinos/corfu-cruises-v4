export interface ScheduleWriteDto {

    // PK
    id: number,
    // FKs
    destinationId: number,
    portId: number,
    // Fields
    date: string,
    maxPax: number,
    time: string,
    isActive: boolean
    // Rowversion
    putAt: string

}
