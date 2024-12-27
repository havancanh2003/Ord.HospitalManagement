import { ConfigStateService, RoutesService, eLayoutType } from '@abp/ng.core';
import { APP_INITIALIZER } from '@angular/core';

export const APP_ROUTE_PROVIDER = [
  {
    provide: APP_INITIALIZER,
    useFactory: configureRoutes,
    deps: [RoutesService, ConfigStateService],
    multi: true,
  },
];

function configureRoutes(routesService: RoutesService, configStateService: ConfigStateService) {
  return () => {
    routesService.add([
      {
        path: '/',
        name: '::Menu:Home',
        iconClass: 'fas fa-home',
        order: 1,
        layout: eLayoutType.application,
      },
      {
        path: '/address',
        name: '::Menu:Address',
        iconClass: 'fas fa-book',
        order: 2,
        layout: eLayoutType.application,
        requiredPolicy: 'AbpIdentity.Roles',
      },
      {
        path: '/address/province',
        name: '::Menu:Province',
        parentName: '::Menu:Address',
        layout: eLayoutType.application,
      },
      {
        path: '/address/district',
        name: '::Menu:District',
        parentName: '::Menu:Address',
        layout: eLayoutType.application,
      },
      {
        path: '/address/ward',
        name: '::Menu:Ward',
        parentName: '::Menu:Address',
        layout: eLayoutType.application,
      },
      //
      {
        path: '/',
        name: '::Menu:Hospital',
        iconClass: 'fas fa-book',
        order: 3,
        layout: eLayoutType.application,
      },
      {
        path: '/manage-hospital',
        name: '::List:Hospital',
        iconClass: 'fas fa-book',
        parentName: '::Menu:Hospital',
        requiredPolicy: 'AbpIdentity.Roles',
        layout: eLayoutType.application,
      },
      {
        path: '/manage-hospital/manage-patient',
        name: '::Action:Patient',
        iconClass: 'fas fa-book',
        parentName: '::Menu:Hospital',
        requiredPolicy: 'HospitalManagement.Patient',
        layout: eLayoutType.application,
      },
    ]);
  };
}
