using Ord.HospitalManagement.DTOs;
using Ord.HospitalManagement.DTOs.Hospital;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Ord.HospitalManagement.IServices
{
    public interface ITenantHospitalAppService : IApplicationService
    {
        Task CreateHospitalAsync(CreateTenantHospitalDto input);
        Task<PagedResultDto<ManageInfoHospital>> GetInfoHospitals(int? pageNumber, int? pageSize);
        Task<HospitalDto> UpdateInfoHospital(int id, CreateUpdateHospitalDto input);
    }
}
