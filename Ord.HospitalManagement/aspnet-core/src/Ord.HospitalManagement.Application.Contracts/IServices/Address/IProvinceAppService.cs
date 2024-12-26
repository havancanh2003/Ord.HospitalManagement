using Microsoft.AspNetCore.Http;
using Ord.HospitalManagement.DataResult;
using Ord.HospitalManagement.DTOs.Address;
using Ord.HospitalManagement.DTOs.Address.ModelFilter;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Ord.HospitalManagement.IServices.Address
{
    public interface IProvinceAppService : ICrudAppService<ProvinceDto,int, CustomePagedAndSortedResultRequestProvinceDto, CreateUpdateProvinceDto>
    {
        Task<ProvinceDto?> GetProvinceByCode(string code);
        Task<DataResult<ProvinceDto>> ImportExcel(IFormFile formFile);
        Task<List<string>> GetAllProvinceCode();

    }
}
