import { Component, EventEmitter, Input, Output } from '@angular/core'

@Component({
    selector: 'table-total-filtered-records',
    templateUrl: './table-total-filtered-records.component.html'
})

export class TableTotalFilteredRecordsComponent {

    @Input() recordCount: number
    @Input() filteredRecordCount: number
    @Input() selectedRecordCount: number
    @Input() showFilteredCount = true
    @Input() mustShowSelectedFilteredCount: boolean = false

    @Output() public resetTableFilters = new EventEmitter()

    public mustResetTableFilters(): void {
        this.resetTableFilters.emit()
    }

}
