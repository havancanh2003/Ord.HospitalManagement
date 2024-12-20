import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: false,
  application: {
    baseUrl,
    name: 'HospitalManagement',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44369/',
    redirectUri: baseUrl,
    clientId: 'HospitalManagement_App',
    responseType: 'code',
    scope: 'offline_access HospitalManagement',
    requireHttps: true,
  },
  apis: {
    default: {
      url: 'https://localhost:44369',
      rootNamespace: 'Ord.HospitalManagement',
    },
  },
} as Environment;
