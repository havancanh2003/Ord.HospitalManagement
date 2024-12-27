import { mapEnumToOptions } from '@abp/ng.core';

export enum LevelWard {
  Ward = 6,
  Commune = 7,
  Township = 8,
}

export const levelWardOptions = mapEnumToOptions(LevelWard);
