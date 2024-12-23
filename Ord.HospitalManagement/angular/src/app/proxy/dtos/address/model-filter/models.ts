import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface CustomePagedAndSortedResultRequestDistrictDto extends PagedAndSortedResultRequestDto {
  provinceCode?: string;
  filterName?: string;
}

export interface CustomePagedAndSortedResultRequestProvinceDto extends PagedAndSortedResultRequestDto {
  filterName?: string;
}

export interface CustomePagedAndSortedResultRequestWardDto extends PagedAndSortedResultRequestDto {
  provinceCode?: string;
  districtCode?: string;
  filterName?: string;
}
