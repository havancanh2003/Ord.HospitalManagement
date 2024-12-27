import type { LevelDistrict } from '../../enums/level-district.enum';
import type { LevelProvince } from '../../enums/level-province.enum';
import type { LevelWard } from '../../enums/level-ward.enum';
import type { AuditedEntityDto } from '@abp/ng.core';

export interface CreateUpdateDistrictDto {
  name: string;
  levelDistrict: LevelDistrict;
  provinceCode: string;
}

export interface CreateUpdateProvinceDto {
  name: string;
  levelProvince: LevelProvince;
}

export interface CreateUpdateWardDto {
  name: string;
  levelWard: LevelWard;
  provinceCode: string;
  districtCode: string;
}

export interface DistrictDto extends AuditedEntityDto<number> {
  code?: string;
  name?: string;
  provinceCode?: string;
  levelDistrict: LevelDistrict;
}

export interface ModelDistrictCodeProvinCodeMap {
  districtCode?: string;
  provinceCode?: string;
}

export interface ProvinceDto extends AuditedEntityDto<number> {
  code?: string;
  name?: string;
  levelProvince: LevelProvince;
}

export interface WardDto extends AuditedEntityDto<number> {
  code?: string;
  name?: string;
  levelWard: LevelWard;
  districtCode?: string;
  provinceCode?: string;
}
