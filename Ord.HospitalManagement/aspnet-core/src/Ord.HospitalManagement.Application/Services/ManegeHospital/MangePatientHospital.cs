using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Ord.HospitalManagement.DomainServices;
using Ord.HospitalManagement.DTOs;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Ord.HospitalManagement.Services.ManegeHospital
{
    public class MangePatientHospital : ApplicationService, IMangePatientHospital
    {
        private readonly IRepository<Patient, int> _repository;
        private readonly IUserHospitalSerivice _userHospitalSerivice;
        private readonly IGenerateCode _generateCode;
        private readonly DapperRepo.DapperRepo _dapper;

        public MangePatientHospital(IRepository<Patient, int> repository, IGenerateCode generateCode, DapperRepo.DapperRepo dapper, IUserHospitalSerivice userHospitalSerivice)
        {
            _repository = repository;
            _userHospitalSerivice = userHospitalSerivice;
            _generateCode = generateCode;
            _dapper = dapper;
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
        public async Task<PatientDto> UpdatePatiendAsync(int id, CreateUpdatePatientDto input)
        {

            var patient = await _repository.GetAsync(id);
            ObjectMapper.Map(input, patient);
            await _repository.UpdateAsync(patient);

            return ObjectMapper.Map<Patient, PatientDto>(patient);
        }

        public async Task<PagedResultDto<PatientDto>> GetAllPatientByFilter(int? pageNumber, int? pageSize, string? name, string? code)
        {
            var hId = await GetCurrentHospitalIdAsync();
            int currentPageNumber = pageNumber ?? 1;
            int currentPageSize = pageSize ?? 10;
            int offset = (currentPageNumber - 1) * currentPageSize ;

            var countQuery = @"SELECT COUNT(*) FROM Patient WHERE HospitalId = @HospitalId";
            var baseQuery = @"SELECT HospitalId, Code, Fullname, ProvinceCode, DistrictCode, WardCode, DetailAddress,Birthday,MedicalHistory
                            FROM Patient
                            WHERE HospitalId = @HospitalId";

            if (!string.IsNullOrEmpty(name))
            {
                baseQuery += " AND Fullname LIKE @Name";
                countQuery += " AND Fullname LIKE @Name";
            }

            if (!string.IsNullOrEmpty(code))
            {
                baseQuery += " AND Code LIKE @Code";
                countQuery += " AND Code LIKE @Code";
            }

            baseQuery += @" LIMIT @PageSize OFFSET @Offset";
            countQuery += $"; {baseQuery}";
            var parameters = new
            {
                HospitalId = hId,
                Name = $"%{name}%",
                Code = $"%{code}%",
                PageSize = currentPageSize,
                Offset = offset
            };
            var list = await _dapper.QueryMultiGetAsync<PatientDto>(countQuery, parameters);
            return new PagedResultDto<PatientDto>(list.total, list.lists.ToList());
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
