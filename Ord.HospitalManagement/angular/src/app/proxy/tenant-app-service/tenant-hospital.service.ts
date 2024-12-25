import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { CreateTenantHospitalDto, CreateUpdateHospitalDto, HospitalDto } from '../dtos/hospital/models';
import type { ManageInfoHospital } from '../dtos/models';

@Injectable({
  providedIn: 'root',
})
export class TenantHospitalService {
  apiName = 'Default';
  

  createHospital = (input: CreateTenantHospitalDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/app/tenant-hospital/hospital',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  getInfoHospitalsByPageNumberAndPageSize = (pageNumber: number, pageSize: number, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<ManageInfoHospital>>({
      method: 'GET',
      url: '/api/app/tenant-hospital/info-hospitals',
      params: { pageNumber, pageSize },
    },
    { apiName: this.apiName,...config });
  

  updateInfoHospitalByIdAndInput = (id: number, input: CreateUpdateHospitalDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, HospitalDto>({
      method: 'PUT',
      url: `/api/app/tenant-hospital/${id}/info-hospital`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
