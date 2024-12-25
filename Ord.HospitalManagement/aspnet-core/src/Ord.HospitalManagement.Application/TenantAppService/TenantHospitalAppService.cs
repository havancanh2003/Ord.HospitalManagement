﻿using Microsoft.AspNetCore.Identity;
using Ord.HospitalManagement.DapperRepo;
using Ord.HospitalManagement.DomainServices;
using Ord.HospitalManagement.DTOs;
using Ord.HospitalManagement.DTOs.Address;
using Ord.HospitalManagement.DTOs.Hospital;
using Ord.HospitalManagement.Entities;
using Ord.HospitalManagement.Entities.Address;
using Ord.HospitalManagement.IServices;
using Ord.HospitalManagement.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.ObjectMapping;
using Volo.Abp.TenantManagement;
using Volo.Abp.Uow;

namespace Ord.HospitalManagement.TenantAppService
{
    public class TenantHospitalAppService : ApplicationService, ITenantHospitalAppService
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IGenerateCode _generateCode;
        private readonly IRepository<Hospital, int> _repository;
        private readonly IRepository<UserHospital, int> _userHospitalRepository;
        private readonly IdentityUserManager _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DapperRepo.DapperRepo _dapper;

        public TenantHospitalAppService(
            IUnitOfWorkManager unitOfWorkManager,
            IdentityUserManager userManager,
            IGenerateCode generateCode,
            DapperRepo.DapperRepo dapper,
            RoleManager<IdentityRole> roleManager,
            IRepository<UserHospital, int> userHospitalRepository,
            IRepository<Hospital, int> repository
            )
        {
            _unitOfWorkManager = unitOfWorkManager;
            _userManager = userManager;
            _generateCode = generateCode;
            _repository = repository;
            _dapper = dapper;
            _roleManager = roleManager;
            _userHospitalRepository = userHospitalRepository;
        }

        public async Task CreateHospitalAsync(CreateTenantHospitalDto input)
        {
            try
            {
                if (input == null)
                    throw new ArgumentNullException();
                if (input.userHospital == null)
                    throw new ArgumentNullException();
                if (input.createHospital == null)
                    throw new ArgumentNullException();
                var role = await _roleManager.FindByNameAsync(RolesConstant.AdminHospital);
                using (var uow = _unitOfWorkManager.Begin())
                {
                    try
                    {
                        var adminUser = new IdentityUser(
                                    GuidGenerator.Create(),
                                    input.userHospital.UserName,
                                    input.userHospital.EmailAddress, null
                                );
                        adminUser.AddRole(role!.Id);

                        var createUserResult = await _userManager.CreateAsync(adminUser, input.userHospital.Password);
                        if (!createUserResult.Succeeded)
                        {
                            throw new UserFriendlyException("Failed to create admin user for the hospital.");
                        }
                        var hospitalEntity = ObjectMapper.Map<CreateUpdateHospitalDto, Hospital>(input.createHospital);
                        hospitalEntity.Code = _generateCode.AutoGenerateCode(PrefixGencode.PrefixGencode.HOSP);

                        await _repository.InsertAsync(hospitalEntity);
                        await _unitOfWorkManager.Current!.SaveChangesAsync();

                        var adminHopital = new UserHospital
                        {
                            UserId = adminUser.Id,
                            HospitalId = hospitalEntity.Id
                        };
                        //throw new UserFriendlyException("Failed to create admin user for the hospital.");
                        await _userHospitalRepository.InsertAsync(adminHopital);
                        await uow.CompleteAsync();
                    }
                    catch
                    {
                        await uow.RollbackAsync();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception (ex.Message);
            }
        }
        public async Task<PagedResultDto<ManageInfoHospital>> GetInfoHospitals(int? pageNumber, int? pageSize)
        {
            int currentPageNumber = pageNumber ?? 1;
            int currentPageSize = pageSize ?? 10;
            int offset = (currentPageNumber - 1) * currentPageSize;
            var limitClause = $"LIMIT {currentPageSize} OFFSET {offset}";

            var query = $@"
                        SELECT COUNT(*) FROM Hospital;
                        SELECT 
                            uh.HospitalId,
                            h.HospitalName,
                            h.Code AS HospitalCode,
                            h.ProvinceCode,
                            h.DistrictCode,
                            h.WardCode,
                            h.HospitalDetailAddress,
                            u.UserName AS AdminHospitalName,
                            h.Hotline,
                            uh.UserId
                        FROM UserHospital uh
                        INNER JOIN Hospital h ON uh.HospitalId = h.Id
                        INNER JOIN AbpUsers u ON u.Id = uh.UserId
                        ORDER BY h.HospitalName
                        {limitClause}";

            var result = await _dapper.QueryMultiGetAsync<ManageInfoHospital>(query);

            return new PagedResultDto<ManageInfoHospital>(
                result.total,
                result.lists.ToList()
            );
        }
        public async Task<HospitalDto> UpdateInfoHospital(int id, CreateUpdateHospitalDto input)
        {
            var hospital = await _repository.GetAsync(id);
            ObjectMapper.Map(input,hospital);

            await _repository.UpdateAsync(hospital);
            return ObjectMapper.Map<Hospital, HospitalDto>(hospital);
        }
    }
}
