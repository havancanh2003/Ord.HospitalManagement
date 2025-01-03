﻿using Microsoft.AspNetCore.Http;
using Ord.HospitalManagement.DataResult;
using Ord.HospitalManagement.DTOs.Address;
using Ord.HospitalManagement.DTOs.Address.ModelFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Ord.HospitalManagement.IServices.Address
{
    public interface IDistrictAppService : ICrudAppService<DistrictDto,int,CustomePagedAndSortedResultRequestDistrictDto,CreateUpdateDistrictDto>
    {
        Task<DistrictDto?> GetDistrictByCode(string code);
        Task<DataResult<DistrictDto>> ImportExcel(IFormFile formFile);
        Task<List<ModelDistrictCodeProvinCodeMap>> GetAllDistrictCode();

    }
}
