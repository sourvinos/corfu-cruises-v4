export interface EmailQueueDto {

    initiator: string
    entityId?: string
    fromDate?: string,
    toDate?: string
    customerId?: number
    priority: number
    isSent: boolean

}
