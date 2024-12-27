import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { DataResult } from '../data-result/models';
import type { CustomePagedAndSortedResultRequestWardDto } from '../dtos/address/model-filter/models';
import type { CreateUpdateWardDto, WardDto } from '../dtos/address/models';
import type { IFormFile } from '../microsoft/asp-net-core/http/models';

@Injectable({
  providedIn: 'root',
})
export class WardService {
  apiName = 'Default';

  create = (input: CreateUpdateWardDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, WardDto>(
      {
        method: 'POST',
        url: '/api/app/ward',
        body: input,
      },
      { apiName: this.apiName, ...config }
    );

  delete = (id: number, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/ward/${id}`,
      },
      { apiName: this.apiName, ...config }
    );

  get = (id: number, config?: Partial<Rest.Config>) =>
    this.restService.request<any, WardDto>(
      {
        method: 'GET',
        url: `/api/app/ward/${id}`,
      },
      { apiName: this.apiName, ...config }
    );

  getList = (input: CustomePagedAndSortedResultRequestWardDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<WardDto>>(
      {
        method: 'GET',
        url: '/api/app/ward',
        params: {
          provinceCode: input.provinceCode,
          districtCode: input.districtCode,
          filterName: input.filterName,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config }
    );

  importExcelWardByFormFile = (formFile: FormData, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DataResult<WardDto>>(
      {
        method: 'POST',
        url: '/api/app/ward/import-excel-ward',
        body: formFile,
      },
      { apiName: this.apiName, ...config }
    );

  update = (id: number, input: CreateUpdateWardDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, WardDto>(
      {
        method: 'PUT',
        url: `/api/app/ward/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config }
    );

  constructor(private restService: RestService) {}
}
