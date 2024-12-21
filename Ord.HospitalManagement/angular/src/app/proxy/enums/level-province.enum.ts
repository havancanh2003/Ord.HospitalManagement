import { mapEnumToOptions } from '@abp/ng.core';

export enum LevelProvince {
  Province = 1,
  CentralCity = 2,
}

export const levelProvinceOptions = mapEnumToOptions(LevelProvince);
