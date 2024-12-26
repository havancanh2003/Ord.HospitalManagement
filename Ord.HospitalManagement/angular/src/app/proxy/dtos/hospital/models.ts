import type { AuditedEntityDto } from '@abp/ng.core';

export interface CreateUpdatePatientDto {
  fullname: string;
  provinceCode?: string;
  districtCode?: string;
  wardCode?: string;
  detailAddress?: string;
  birthday?: string;
  medicalHistory?: string;
}

export interface PatientDto extends AuditedEntityDto<number> {
  hospitalId: number;
  code?: string;
  fullname?: string;
  provinceCode?: string;
  districtCode?: string;
  wardCode?: string;
  detailAddress?: string;
  birthday?: string;
  medicalHistory?: string;
}

export interface CreateTenantHospitalDto {
  userHospital: CreateUserHospital;
  createHospital: CreateUpdateHospitalDto;
}

export interface CreateUpdateHospitalDto {
  hospitalName: string;
  provinceCode?: string;
  districtCode?: string;
  wardCode?: string;
  hospitalDetailAddress?: string;
  hospitalDescription?: string;
  hotline: string;
}

export interface CreateUserHospital {
  userName: string;
  emailAddress: string;
  password: string;
}

export interface HospitalDto extends AuditedEntityDto<number> {
  hospitalName?: string;
  hotline?: string;
  code?: string;
  provinceCode?: string;
  districtCode?: string;
  wardCode?: string;
  hospitalDetailAddress?: string;
  hospitalDescription?: string;
}
