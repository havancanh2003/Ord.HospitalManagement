import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface CustomePagedAndSortedResultRequestProvinceDto extends PagedAndSortedResultRequestDto {
  filterName?: string;
}
