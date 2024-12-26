import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { DataResult } from '../data-result/models';
import type { CustomePagedAndSortedResultRequestDistrictDto } from '../dtos/address/model-filter/models';
import type { CreateUpdateDistrictDto, DistrictDto } from '../dtos/address/models';
import type { IFormFile } from '../microsoft/asp-net-core/http/models';

@Injectable({
  providedIn: 'root',
})
export class DistrictService {
  apiName = 'Default';

  create = (input: CreateUpdateDistrictDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DistrictDto>(
      {
        method: 'POST',
        url: '/api/app/district',
        body: input,
      },
      { apiName: this.apiName, ...config }
    );

  delete = (id: number, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/district/${id}`,
      },
      { apiName: this.apiName, ...config }
    );

  get = (id: number, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DistrictDto>(
      {
        method: 'GET',
        url: `/api/app/district/${id}`,
      },
      { apiName: this.apiName, ...config }
    );

  getDistrictByCodeByCode = (code: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DistrictDto>(
      {
        method: 'GET',
        url: '/api/app/district/district-by-code',
        params: { code },
      },
      { apiName: this.apiName, ...config }
    );

  getList = (input: CustomePagedAndSortedResultRequestDistrictDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<DistrictDto>>(
      {
        method: 'GET',
        url: '/api/app/district',
        params: {
          provinceCode: input.provinceCode,
          filterName: input.filterName,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config }
    );

  importExcelByFormFile = (formFile: FormData, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DataResult<DistrictDto>>(
      {
        method: 'POST',
        url: '/api/app/district/import-excel',
        body: formFile,
      },
      { apiName: this.apiName, ...config }
    );

  update = (id: number, input: CreateUpdateDistrictDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DistrictDto>(
      {
        method: 'PUT',
        url: `/api/app/district/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config }
    );

  constructor(private restService: RestService) {}
}
