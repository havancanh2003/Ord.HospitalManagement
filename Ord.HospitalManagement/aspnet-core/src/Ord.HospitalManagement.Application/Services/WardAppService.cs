using Ord.HospitalManagement.DomainServices;
using Ord.HospitalManagement.DTOs.Address;
using Ord.HospitalManagement.DTOs.Address.ModelFilter;
using Ord.HospitalManagement.Entities.Address;
using Ord.HospitalManagement.IServices.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace Ord.HospitalManagement.Services
{
    public class WardAppService : CrudAppService<Ward, WardDto, int, CustomePagedAndSortedResultRequestWardDto, CreateUpdateWardDto>, IWardAppService
    {
        private readonly IGenerateCode _generateCode;
        private readonly IProvinceAppService _provinceAppService;
        private readonly IDistrictAppService _districtAppService;

        public WardAppService(IRepository<Ward, int> repository, IGenerateCode generateCode, IDistrictAppService districtAppService,IProvinceAppService provinceAppService) : base(repository)
        {
            _generateCode = generateCode;
            _districtAppService = districtAppService;
            _provinceAppService = provinceAppService;
        }
        public async override Task<PagedResultDto<WardDto>> GetListAsync(CustomePagedAndSortedResultRequestWardDto input)
        {
            var queryable = await Repository.GetQueryableAsync();

            var filteredQuery = queryable
                .WhereIf(!input.ProvinceCode.IsNullOrEmpty(), x => x.ProvinceCode.Contains(input.ProvinceCode))
                .WhereIf(!input.DistrictCode.IsNullOrEmpty(), x => x.DistrictCode.Contains(input.DistrictCode))
                .WhereIf(!input.FilterName.IsNullOrEmpty(), x => x.Name.Contains(input.FilterName));

            var totalCount = await AsyncExecuter.CountAsync(filteredQuery);

            var wards = await AsyncExecuter.ToListAsync(
                filteredQuery
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount)
            );
            var wardDtos = ObjectMapper.Map<List<Ward>, List<WardDto>>(wards);
            return new PagedResultDto<WardDto>(
                totalCount,
                wardDtos
            );
        }
        public override async Task<WardDto> CreateAsync(CreateUpdateWardDto input)
        {
            if (string.IsNullOrEmpty(input.ProvinceCode))
                throw new Exception();

            if (string.IsNullOrEmpty(input.DistrictCode))
                throw new Exception();

            var checkProvince = await _provinceAppService.GetProvinceByCode(input.ProvinceCode);
            if (checkProvince == null)
                throw new Exception();

            var checkDistrict = await _districtAppService.GetDistrictByCode(input.DistrictCode);
            if (checkDistrict == null)
                throw new Exception();

            var ward = ObjectMapper.Map<CreateUpdateWardDto, Ward>(input);
            ward.Code = _generateCode.AutoGenerateCode(PrefixGencode.PrefixGencode.WARD);

            await Repository.InsertAsync(ward);
            return ObjectMapper.Map<Ward, WardDto>(ward);
        }

        public override async Task<WardDto> UpdateAsync(int id, CreateUpdateWardDto input)
        {
            if (string.IsNullOrEmpty(input.ProvinceCode))
                throw new Exception();

            if (string.IsNullOrEmpty(input.DistrictCode))
                throw new Exception();

            var checkProvince = await _provinceAppService.GetProvinceByCode(input.ProvinceCode);
            if (checkProvince == null)
                throw new Exception();

            var checkDistrict = await _districtAppService.GetDistrictByCode(input.DistrictCode);
            if (checkDistrict == null)
                throw new Exception();

            var existingWard = await Repository.GetAsync(id);
            if (existingWard == null)
                throw new Exception();

            ObjectMapper.Map(input, existingWard);
            await Repository.UpdateAsync(existingWard);

            return ObjectMapper.Map<Ward, WardDto>(existingWard);
        }
    }
}
