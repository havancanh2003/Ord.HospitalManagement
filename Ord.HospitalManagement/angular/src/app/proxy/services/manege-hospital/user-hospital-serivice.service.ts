import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class UserHospitalSeriviceService {
  apiName = 'Default';
  

  getHospitalIdByCurrenAdminId = (currenAdminId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, number>({
      method: 'GET',
      url: `/api/app/user-hospital-serivice/hospital-id/${currenAdminId}`,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
