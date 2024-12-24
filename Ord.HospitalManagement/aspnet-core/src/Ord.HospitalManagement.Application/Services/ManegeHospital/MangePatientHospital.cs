using Ord.HospitalManagement.DomainServices;
using Ord.HospitalManagement.DTOs.Address;
using Ord.HospitalManagement.DTOs.Hospital;
using Ord.HospitalManagement.Entities;
using Ord.HospitalManagement.Entities.Address;
using Ord.HospitalManagement.IServices.Hospital;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace Ord.HospitalManagement.Services.ManegeHospital
{
    public class MangePatientHospital : ApplicationService, IMangePatientHospital
    {
        private readonly IRepository<Patient, int> _repository;
        private readonly IUserHospitalSerivice _userHospitalSerivice;
        private readonly IGenerateCode _generateCode;


        public MangePatientHospital(IRepository<Patient, int> repository, IGenerateCode generateCode, IUserHospitalSerivice userHospitalSerivice)
        {
            _repository = repository;
            _userHospitalSerivice = userHospitalSerivice;
            _generateCode = generateCode;
        }
        public async Task<PatientDto> CreatePatiendAsync(CreateUpdatePatientDto input)
        {
            var hId = await GetCurrentHospitalIdAsync();
            var patient = ObjectMapper.Map<CreateUpdatePatientDto, Patient>(input);
            patient.HospitalId = hId;
            patient.Code = _generateCode.AutoGenerateCode(PrefixGencode.PrefixGencode.PATI);

            await _repository.InsertAsync(patient);
            return ObjectMapper.Map<Patient, PatientDto>(patient);
        }
        public async Task<PatientDto> UpdatePatiendAsync(int id,CreateUpdatePatientDto input)
        {
            
            var patient = await _repository.GetAsync(id);
            ObjectMapper.Map(input,patient);
            await _repository.UpdateAsync(patient);

            return ObjectMapper.Map<Patient, PatientDto>(patient);
        }

        public async Task<PagedResultDto<PatientDto>> GetAllPatient()
        {
            var hId = await GetCurrentHospitalIdAsync();
            var queryable = await _repository.GetQueryableAsync();
            var list = await AsyncExecuter.ToListAsync(
                queryable.Where(p => p.HospitalId == hId)
            );
            var dtos = ObjectMapper.Map<List<Patient>, List<PatientDto>>(list);
            var totalCount = await _repository.GetCountAsync();

            return new PagedResultDto<PatientDto>(
                totalCount,
                dtos
            );
        }
        private async Task<int> GetCurrentHospitalIdAsync()
        {
            var currentUserId = CurrentUser.Id;
            if (currentUserId == null)
            {
                throw new UserFriendlyException("Current user is not logged in.");
            }
            var hospitalId = await _userHospitalSerivice.GetHospitalId(currentUserId.Value);
            if (hospitalId == null)
            {
                throw new UserFriendlyException("User is not assigned to a hospital.");
            }
            return hospitalId.Value;
        }
    }
}
