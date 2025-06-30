import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
// Custom
import { AuthGuardService } from 'src/app/shared/services/auth-guard.service'
import { CustomersResolver } from '../resolvers/customers-resolver'
import { DestinationsResolver } from '../resolvers/destinations-resolver'
import { DriversResolver } from '../resolvers/drivers-resolver'
import { NationalitiesResolver } from '../resolvers/nationalities-resolver'
import { PortsResolver } from '../resolvers/ports-resolver'
import { ShipsResolver } from '../resolvers/ships-resolver'
import { StatisticsComponent } from '../../user-interface/statistics.component'
import { YTDResolver as YTDResolver } from '../resolvers/ytd-resolver'

const routes: Routes = [
    {
        path: '', component: StatisticsComponent, canActivate: [AuthGuardService], resolve: {
            ytd: YTDResolver,
            customers: CustomersResolver,
            destinations: DestinationsResolver,
            drivers: DriversResolver,
            ports: PortsResolver,
            ships: ShipsResolver,
            nationalities: NationalitiesResolver
        }, runGuardsAndResolvers: 'always'
    }
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class StatisticsRoutingModule { }
