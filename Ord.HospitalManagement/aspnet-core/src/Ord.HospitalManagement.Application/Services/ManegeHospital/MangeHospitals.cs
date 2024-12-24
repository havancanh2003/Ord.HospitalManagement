using Ord.HospitalManagement.DTOs.Hospital;
using Ord.HospitalManagement.Entities;
using Ord.HospitalManagement.IServices.Hospital;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace Ord.HospitalManagement.Services.ManegeHospital
{
    public class MangeHospitals : ApplicationService, IMangeHospitals
    {
        //private readonly IRepository<Patient,int> _repository;
        
        //public MangeHospitals(IRepository<Patient, int> repository)
        //{
        //    _repository = repository;
        //}
        //public Task CreatePatiendAsync()
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task<PagedResultDto<PatientDto>> GetAllPatient()
        //{
        //    var totalCount = await _repository.GetCountAsync();

        //    return new PagedResultDto<PatientDto>(
        //        totalCount,
        //        new List<PatientDto>()
        //    );
        //}
    }
}
