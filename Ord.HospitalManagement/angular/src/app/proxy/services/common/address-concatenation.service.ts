import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AddressConcatenationService {
  apiName = 'Default';
  

  detailAddressByPCodeAndDCodeAndWCodeAndDetail = (pCode: string, dCode: string, wCode: string, detail: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, string>({
      method: 'POST',
      responseType: 'text',
      url: '/api/app/address-concatenation/detail-address',
      params: { pCode, dCode, wCode, detail },
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
