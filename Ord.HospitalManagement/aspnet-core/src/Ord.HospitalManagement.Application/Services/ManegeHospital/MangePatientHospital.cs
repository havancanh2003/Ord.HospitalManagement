using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Ord.HospitalManagement.DomainServices;
using Ord.HospitalManagement.DTOs;
using Ord.HospitalManagement.DTOs.Hospital;
using Ord.HospitalManagement.Entities;
using Ord.HospitalManagement.IServices.Hospital;
using Ord.HospitalManagement.Roles;
using Ord.HospitalManagement.Services.Common;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace Ord.HospitalManagement.Services.ManegeHospital
{
    [Authorize(Roles = RolesConstant.AdminHospital)]
    public class MangePatientHospital : ApplicationService, IMangePatientHospital
    {
        private readonly IRepository<Patient, int> _repository;
        private readonly IUserHospitalSerivice _userHospitalSerivice;
        private readonly IGenerateCode _generateCode;
        private readonly DapperRepo.DapperRepo _dapper;
        private readonly AddressConcatenation _addressConcatenation;

        public MangePatientHospital(IRepository<Patient, int> repository, IGenerateCode generateCode, DapperRepo.DapperRepo dapper, AddressConcatenation addressConcatenation, IUserHospitalSerivice userHospitalSerivice)
        {
            _repository = repository;
            _userHospitalSerivice = userHospitalSerivice;
            _generateCode = generateCode;
            _dapper = dapper;
            _addressConcatenation = addressConcatenation;
        }
        public async Task<PatientDto> CreatePatientAsync(CreateUpdatePatientDto input)
        {
            try
            {
                var hId = await GetCurrentHospitalIdAsync();
                var patient = ObjectMapper.Map<CreateUpdatePatientDto, Patient>(input);
                patient.HospitalId = hId;
                patient.Code = _generateCode.AutoGenerateCode(PrefixGencode.PrefixGencode.PATI);
                await _repository.InsertAsync(patient);

                return ObjectMapper.Map<Patient, PatientDto>(patient);
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        public async Task<PatientDto> UpdatePatientAsync(int id, CreateUpdatePatientDto input)
        {
            try
            {
                var patient = await _repository.GetAsync(id);
                if (patient == null)
                    throw new ArgumentNullException("Không tồn tại người bệnh trong hệ thống");

                ObjectMapper.Map(input, patient);
                await _repository.UpdateAsync(patient);

                return ObjectMapper.Map<Patient, PatientDto>(patient);
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        public async Task DeletePatientAsync(int id)
        {
            try
            {
                var patient = await _repository.GetAsync(id);
                if (patient == null)
                    throw new ArgumentNullException("Không tồn tại người bệnh trong hệ thống");

                await _repository.DeleteAsync(patient);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<PatientDto> GetPatientByIdAsync(int id)
        {
            try
            {
                var patient = await _repository.GetAsync(id);
                if (patient == null)
                    throw new ArgumentNullException("Không tồn tại thông tin bệnh nhân trong hệ thống");
                return ObjectMapper.Map<Patient, PatientDto>(patient);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PagedResultDto<PatientDto>> GetAllPatientByFilter(int? pageNumber, int? pageSize, string? name, string? code)
        {
            try
            {
                var hId = await GetCurrentHospitalIdAsync();
                int currentPageNumber = pageNumber ?? 1;
                int currentPageSize = pageSize ?? 10;
                int offset = (currentPageNumber - 1) * currentPageSize;

                var countQuery = @"SELECT COUNT(*) FROM Patient WHERE HospitalId = @HospitalId";
                var baseQuery = @"SELECT Id, HospitalId, Code, Fullname, ProvinceCode, DistrictCode, WardCode, DetailAddress,Birthday,MedicalHistory
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
                var result = await _dapper.QueryMultiGetAsync<PatientDto>(countQuery, parameters);
                foreach (var patient in result.lists)
                {
                    patient.DetailAddress = await _addressConcatenation.DetailAddress(
                        patient.ProvinceCode,
                        patient.DistrictCode,
                        patient.WardCode,
                        patient.DetailAddress
                    );
                }
                return new PagedResultDto<PatientDto>(result.total, result.lists.ToList());
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
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
