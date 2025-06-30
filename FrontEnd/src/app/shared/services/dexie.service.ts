import Dexie from 'dexie'
import { Injectable } from '@angular/core'
// Custom
import { CustomerHttpService } from 'src/app/features/reservations/customers/classes/services/customer-http.service'
import { DocumentTypeHttpService } from 'src/app/features/sales/documentTypes/classes/services/documentType-http.service'

@Injectable({ providedIn: 'root' })

export class DexieService extends Dexie {

    constructor() {
        super('CorfuCruisesDB')
        this.version(3).stores({
            banks: 'id, description',
            coachRoutes: 'id, abbreviation',
            crewSpecialties: 'id, description',
            customers: 'id, description',
            customersCriteria: 'id, description',
            destinations: 'id, description',
            destinationsCriteria: 'id, description',
            documentTypesInvoice: 'id, abbreviation, isDefault',
            documentTypesReceipt: 'id, abbreviation',
            drivers: 'id, description',
            genders: 'id, description',
            nationalities: 'id, description',
            paymentMethods: 'id, description, isDefault',
            pickupPoints: 'id, description',
            ports: 'id, description',
            portsCriteria: 'id, description',
            shipOwners: 'id, description, descriptionEn, isMyData, isOxygen',
            ships: 'id, description, isActive',
            shipsCriteria: 'id, description',
            taxOffices: 'id, description'
        })
        this.open()
    }

    public populateTable(table: string, httpService: any): void {
        httpService.getForBrowser().subscribe((records: any) => {
            this.table(table)
                .clear().then(() => {
                    this.table(table)
                        .bulkAdd(records)
                        .catch(Dexie.BulkError, () => { })
                })
        })
    }

    public populateCriteria(table: string, httpService: any): void {
        httpService.getForCriteria().subscribe((records: any) => {
            this.table(table)
                .clear().then(() => {
                    this.table(table)
                        .bulkAdd(records)
                        .catch(Dexie.BulkError, () => { })
                })
        })
    }

    public populateNewTable(table: string, customerHttpService: CustomerHttpService): void {
        customerHttpService.getBrowserStorage().subscribe((records: any) => {
            this.table(table)
                .clear().then(() => {
                    this.table(table)
                        .bulkAdd(records)
                        .catch(Dexie.BulkError, () => { })
                })
        })
    }

    public populateDocumentTypesTable(table: string, documentTypeHttpService: DocumentTypeHttpService, discriminatorId: number): void {
        documentTypeHttpService.getBrowserStorage(discriminatorId).subscribe((records: any) => {
            this.table(table)
                .clear().then(() => {
                    this.table(table)
                        .bulkAdd(records)
                        .catch(Dexie.BulkError, () => { })
                })
        })
    }

    public async getById(table: string, id: number): Promise<any> {
        return await this.table(table).get({ id: id })
    }

    public async getByDescription(table: string, description: string): Promise<any> {
        return await this.table(table).get({ description: description })
    }

    public async getByDefault(table: string, field: string): Promise<any> {
        return this.table(table).filter(x => x[field]).first()
    }

    public async getDefaultDocumentType(table: string, shipId: number): Promise<any> {
        return this.table(table).filter(x => x.ship.id == shipId && x.isDefault).first()
    }

    public update(table: string, item: any): void {
        this.table(table).put(item)
    }

    public remove(table: string, id: any): void {
        this.table(table).delete(id)
    }

}

export const db = new DexieService()
