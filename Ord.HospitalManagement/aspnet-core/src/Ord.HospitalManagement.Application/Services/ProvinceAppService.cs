using Ord.HospitalManagement.DomainServices;
using Ord.HospitalManagement.DTOs.Address;
using Ord.HospitalManagement.DTOs.Address.ModelFilter;
using Ord.HospitalManagement.Entities.Address;
using Ord.HospitalManagement.IServices.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Ord.HospitalManagement.Services
{
    public class ProvinceAppService :
                    CrudAppService<Province, ProvinceDto, int, CustomePagedAndSortedResultRequestProvinceDto, CreateUpdateProvinceDto>,
                    IProvinceAppService
    {
        private readonly IGenerateCode _generateCode;
        public ProvinceAppService(IRepository<Province, int> repository, IGenerateCode generateCode) : base(repository)
        {
            _generateCode = generateCode;
        }

        public async override Task<PagedResultDto<ProvinceDto>>GetListAsync(CustomePagedAndSortedResultRequestProvinceDto input)
        {
            var queryable = await Repository.GetQueryableAsync();

            var provinces = await AsyncExecuter.ToListAsync(
                queryable
                    .WhereIf(!input.FilterName.IsNullOrEmpty(), x => x.Name.Contains(input.FilterName)) // apply filtering
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount)
            );
            //Convert to DTOs
            var provinceDtos = ObjectMapper.Map<List<Province>, List<ProvinceDto>>(provinces);
            var totalCount = await Repository.GetCountAsync();

            return new PagedResultDto<ProvinceDto>(
                totalCount,
                provinceDtos
            );
        }

        public override async Task<ProvinceDto> CreateAsync(CreateUpdateProvinceDto input)
        {
            var province = ObjectMapper.Map<CreateUpdateProvinceDto, Province>(input);
            province.Code = _generateCode.AutoGenerateCode(PrefixGencode.PrefixGencode.PROV);

            await Repository.InsertAsync(province);
            return ObjectMapper.Map<Province, ProvinceDto>(province);
        }
        public override async Task<ProvinceDto> UpdateAsync(int id, CreateUpdateProvinceDto input)
        {
            var existingProvince = await Repository.GetAsync(id);
            ObjectMapper.Map(input, existingProvince);
            await Repository.UpdateAsync(existingProvince);

            return ObjectMapper.Map<Province, ProvinceDto>(existingProvince);
        }
    }
}
