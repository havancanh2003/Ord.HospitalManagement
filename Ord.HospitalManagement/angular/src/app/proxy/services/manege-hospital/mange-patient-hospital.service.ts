import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { CreateUpdatePatientDto, PatientDto } from '../../dtos/hospital/models';

@Injectable({
  providedIn: 'root',
})
export class MangePatientHospitalService {
  apiName = 'Default';
  

  createPatient = (input: CreateUpdatePatientDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PatientDto>({
      method: 'POST',
      url: '/api/app/mange-patient-hospital/patient',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  deletePatient = (id: number, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/mange-patient-hospital/${id}/patient`,
    },
    { apiName: this.apiName,...config });
  

  getAllPatientByFilterByPageNumberAndPageSizeAndNameAndCode = (pageNumber: number, pageSize: number, name: string, code: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<PatientDto>>({
      method: 'GET',
      url: '/api/app/mange-patient-hospital/patient-by-filter',
      params: { pageNumber, pageSize, name, code },
    },
    { apiName: this.apiName,...config });
  

  getPatientById = (id: number, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PatientDto>({
      method: 'GET',
      url: `/api/app/mange-patient-hospital/${id}/patient-by-id`,
    },
    { apiName: this.apiName,...config });
  

  updatePatient = (id: number, input: CreateUpdatePatientDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PatientDto>({
      method: 'PUT',
      url: `/api/app/mange-patient-hospital/${id}/patient`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
