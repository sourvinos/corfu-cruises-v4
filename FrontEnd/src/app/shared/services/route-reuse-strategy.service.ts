import { Injectable } from '@angular/core'
import { ActivatedRouteSnapshot, BaseRouteReuseStrategy } from '@angular/router'

@Injectable({ providedIn: 'root' })

export class CustomRouteReuseStrategyService extends BaseRouteReuseStrategy {

    public override shouldReuseRoute(future: ActivatedRouteSnapshot): boolean {
        return future.data['reuseComponent']
    }

}