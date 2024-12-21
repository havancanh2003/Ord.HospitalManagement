using Ord.HospitalManagement.DTOs.Address;
using Ord.HospitalManagement.DTOs.Address.ModelFilter;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Ord.HospitalManagement.IServices.Address
{
    public interface IProvinceAppService : ICrudAppService<ProvinceDto,int, CustomePagedAndSortedResultRequestProvinceDto, CreateUpdateProvinceDto>
    {
    }
}
