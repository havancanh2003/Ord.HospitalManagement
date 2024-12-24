using Microsoft.AspNetCore.Identity;
using Ord.HospitalManagement.DomainServices;
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
        private readonly ITenantRepository _tenantRepository;
        private readonly ITenantManager _tenantManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IGenerateCode _generateCode;
        private readonly IRepository<Hospital, int> _repository;
        private readonly IRepository<UserHospital, int> _userHospitalRepository;
        private readonly IdentityUserManager _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public TenantHospitalAppService(
            ITenantRepository tenantRepository,
            ITenantManager tenantManager,
            IUnitOfWorkManager unitOfWorkManager,
            IdentityUserManager userManager,
            IGenerateCode generateCode,
            RoleManager<IdentityRole> roleManager,
            IRepository<UserHospital, int> userHospitalRepository,
            IRepository<Hospital, int> repository
            )
        {
            _tenantRepository = tenantRepository;
            _tenantManager = tenantManager;
            _unitOfWorkManager = unitOfWorkManager;
            _userManager = userManager;
            _generateCode = generateCode;
            _repository = repository;
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
                using (var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: true))
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
                        throw new UserFriendlyException("Failed to create admin user for the hospital.");
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
        //public async Task CreateHospitalAsync(CreateTenantHospitalDto input)
        //{
        //    //try
        //    //{
        //    //    if (input == null)
        //    //        throw new ArgumentNullException();
        //    //    if (input.userHospital == null)
        //    //        throw new ArgumentNullException();
        //    //    if (input.createHospital == null)
        //    //        throw new ArgumentNullException();

        //    //    var role = await _roleManager.FindByNameAsync(RolesConstant.AdminHospital);

        //    //    var tenant = await _tenantManager.CreateAsync(new Guid().ToString());
        //    //    await _tenantRepository.InsertAsync(tenant);

        //    //    using (CurrentTenant.Change(tenant.Id))
        //    //    {
        //    //        var adminUser = new IdentityUser(
        //    //            GuidGenerator.Create(),
        //    //            input.userHospital.UserName,
        //    //            input.userHospital.EmailAddress,
        //    //            tenant.Id
        //    //        );
        //    //        adminUser.AddRole(role!.Id);

        //    //        var createUserResult = await _userManager.CreateAsync(adminUser, input.userHospital.Password);
        //    //        if (!createUserResult.Succeeded)
        //    //        {
        //    //            throw new UserFriendlyException("Failed to create admin user for the hospital.");
        //    //        }

        //    //        //var addToRoleResult = await _userManager.AddToRoleAsync(adminUser, RolesConstant.AdminHospital);
        //    //        //if (!addToRoleResult.Succeeded)
        //    //        //{
        //    //        //    throw new UserFriendlyException($"Failed to add admin role: {string.Join(", ", addToRoleResult.Errors.Select(e => e.Description))}");
        //    //        //}

        //    //        var hoptitalEntity = ObjectMapper.Map<CreateUpdateHospitalDto, Hospital>(input.createHospital);
        //    //        hoptitalEntity.Code = _generateCode.AutoGenerateCode(PrefixGencode.PrefixGencode.HOSP);
        //    //        //hoptitalEntity.UserHospitalId = adminUser.Id

        //    //        await _repository.InsertAsync(hoptitalEntity);
        //    //    }
        //    //    //using (var uow = _unitOfWorkManager.Begin())
        //    //    //{
        //    //    //    try
        //    //    //    {
        //    //    //        await uow.CompleteAsync();

        //    //    //    }
        //    //    //    catch
        //    //    //    {
        //    //    //        await uow.RollbackAsync();
        //    //    //    }
        //    //    //}
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    throw;
        //    //}
        //}

        public async Task<PagedResultDto<HospitalDto>> GetAllHospitals()
        {
            var list = await _repository.GetListAsync();
            var dtos = ObjectMapper.Map<List<Hospital>, List<HospitalDto>>(list);
            var totalCount = await _repository.GetCountAsync();

            return new PagedResultDto<HospitalDto>(
                totalCount,
                dtos
            );
        }
    }
}
