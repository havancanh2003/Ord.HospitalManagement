import type { LevelProvince } from '../../enums/level-province.enum';
import type { AuditedEntityDto } from '@abp/ng.core';

export interface CreateUpdateProvinceDto {
  name: string;
  levelProvince: LevelProvince;
}

export interface ProvinceDto extends AuditedEntityDto<number> {
  code?: string;
  name?: string;
  levelProvince: LevelProvince;
}
