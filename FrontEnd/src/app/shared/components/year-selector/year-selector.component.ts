import { Component, EventEmitter, Input, Output } from '@angular/core'
// Custom
import { InteractionService } from '../../services/interaction.service'

@Component({
    selector: 'year-selector',
    templateUrl: './year-selector.component.html',
    styleUrls: ['./year-selector.component.css']
})

export class YearSelectorComponent {

    //#region variables

    @Input() public year: string
    @Output() public yearEmitter = new EventEmitter()

    public menuItems: string[]
    public years: string[]

    //#endregion

    constructor(private interactionService: InteractionService) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.populateYears()
        this.buildMenu()
    }

    //#endregion

    //#region public methods

    public doNavigationTasks(year: string): any {
        this.yearEmitter.emit(year)
    }

    //#endregion

    //#region private methods

    private buildMenu(): void {
        this.menuItems = []
        this.years.forEach(item => {
            this.menuItems.push(item)
        })
    }

    private populateYears(): void {
        this.years = []
        for (let year = 2022; year < new Date().getFullYear() + 2; year++) {
            this.years.push(year.toString())
        }
    }

    //#endregion

}
