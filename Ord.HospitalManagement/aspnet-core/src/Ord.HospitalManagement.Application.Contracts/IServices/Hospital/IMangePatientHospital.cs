using Ord.HospitalManagement.DTOs.Hospital;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Ord.HospitalManagement.IServices.Hospital
{
    public interface IMangePatientHospital : IApplicationService
    {
        Task<PagedResultDto<PatientDto>> GetAllPatient();
        Task<PatientDto> CreatePatiendAsync(CreateUpdatePatientDto input);
    }
}
