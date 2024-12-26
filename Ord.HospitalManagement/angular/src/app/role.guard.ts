import { ConfigStateService } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  CanMatch,
  GuardResult,
  MaybeAsync,
  Route,
  Router,
  RouterStateSnapshot,
  UrlSegment,
} from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class RoleGuard implements CanActivate, CanMatch {
  constructor(private config: ConfigStateService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): MaybeAsync<GuardResult> {
    const currentUser = this.config.getOne('currentUser');
    if (currentUser?.roles.includes('AdminHospital')) {
      return true;
    }
    this.router.navigate(['/']);
    return false;
  }
  canMatch(route: Route, segments: UrlSegment[]): MaybeAsync<GuardResult> {
    const currentUser = this.config.getOne('currentUser');
    if (currentUser?.roles.includes('admin')) {
      return true;
    }
    this.router.navigate(['/']);
    return false;
  }
}
