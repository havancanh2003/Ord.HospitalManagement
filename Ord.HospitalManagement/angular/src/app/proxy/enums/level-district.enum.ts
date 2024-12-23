import { mapEnumToOptions } from '@abp/ng.core';

export enum LevelDistrict {
  District = 3,
  City = 4,
  Town = 5,
}

export const levelDistrictOptions = mapEnumToOptions(LevelDistrict);
