using Microsoft.AspNetCore.Http;
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
    public interface IWardAppService : ICrudAppService<WardDto,int,CustomePagedAndSortedResultRequestWardDto,CreateUpdateWardDto>
    {
        Task<DataResult<WardDto>> ImportExcelWard(IFormFile formFile);
    }
}
