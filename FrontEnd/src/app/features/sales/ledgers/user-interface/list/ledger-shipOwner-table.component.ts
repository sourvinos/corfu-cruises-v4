import { formatNumber } from '@angular/common'
import { Component, Input } from '@angular/core'
// Custom
import { LedgerCriteriaVM } from '../../classes/view-models/criteria/ledger-criteria-vm'
import { LedgerVM } from '../../classes/view-models/list/ledger-vm'
import { LocalStorageService } from 'src/app/shared/services/local-storage.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'

@Component({
    selector: 'ledgerShipOwnerTable',
    templateUrl: './ledger-shipOwner-table.component.html',
    styleUrls: ['../../../../../../assets/styles/custom/lists.css', './ledger-parent.component.css']
})

export class LedgerShipOwnerTableComponent {

    //#region variables

    @Input() records: LedgerVM[] = []
    @Input() criteria: LedgerCriteriaVM

    public feature = 'salesLedger'

    //#endregion

    constructor(private localStorageService: LocalStorageService, private messageLabelService: MessageLabelService) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.adjustTableHeight()
    }

    //#endregion

    //#region public methods

    public formatNumberToLocale(number: number, decimals = true): string {
        return formatNumber(number, this.localStorageService.getItem('language'), decimals ? '1.2' : '1.0')
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    //#endregion

    //#region private methods

    private adjustTableHeight(): void {
        setTimeout(() => {
            const x = document.getElementsByClassName('table-wrapper') as HTMLCollectionOf<HTMLInputElement>
            for (let i = 0; i < x.length; i++) {
                x[i].style.height = document.getElementById('content').offsetHeight - 150 + 'px'
            }
        }, 100)
    }

    //#endregion

}
