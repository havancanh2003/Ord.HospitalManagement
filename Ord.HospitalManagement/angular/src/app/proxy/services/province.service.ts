import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { DataResult } from '../data-result/models';
import type { CustomePagedAndSortedResultRequestProvinceDto } from '../dtos/address/model-filter/models';
import type { CreateUpdateProvinceDto, ProvinceDto } from '../dtos/address/models';
import type { IFormFile } from '../microsoft/asp-net-core/http/models';

@Injectable({
  providedIn: 'root',
})
export class ProvinceService {
  apiName = 'Default';

  create = (input: CreateUpdateProvinceDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ProvinceDto>(
      {
        method: 'POST',
        url: '/api/app/province',
        body: input,
      },
      { apiName: this.apiName, ...config }
    );

  delete = (id: number, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/province/${id}`,
      },
      { apiName: this.apiName, ...config }
    );

  get = (id: number, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ProvinceDto>(
      {
        method: 'GET',
        url: `/api/app/province/${id}`,
      },
      { apiName: this.apiName, ...config }
    );

  getAllProvinceCode = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, string[]>(
      {
        method: 'GET',
        url: '/api/app/province/province-code',
      },
      { apiName: this.apiName, ...config }
    );

  getList = (input: CustomePagedAndSortedResultRequestProvinceDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<ProvinceDto>>(
      {
        method: 'GET',
        url: '/api/app/province',
        params: {
          filterName: input.filterName,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config }
    );

  getProvinceByCodeByCode = (code: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ProvinceDto>(
      {
        method: 'GET',
        url: '/api/app/province/province-by-code',
        params: { code },
      },
      { apiName: this.apiName, ...config }
    );

  importExcelByFormFile = (formFile: FormData, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DataResult<ProvinceDto>>(
      {
        method: 'POST',
        url: '/api/app/province/import-excel',
        body: formFile,
      },
      { apiName: this.apiName, ...config }
    );

  update = (id: number, input: CreateUpdateProvinceDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ProvinceDto>(
      {
        method: 'PUT',
        url: `/api/app/province/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config }
    );

  constructor(private restService: RestService) {}
}
