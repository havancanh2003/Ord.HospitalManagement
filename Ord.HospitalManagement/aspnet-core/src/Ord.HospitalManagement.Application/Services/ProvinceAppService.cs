using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using Ord.HospitalManagement.DapperRepo;
using Ord.HospitalManagement.DataResult;
using Ord.HospitalManagement.DomainServices;
using Ord.HospitalManagement.DTOs.Address;
using Ord.HospitalManagement.DTOs.Address.ModelFilter;
using Ord.HospitalManagement.Entities.Address;
using Ord.HospitalManagement.Enums;
using Ord.HospitalManagement.IServices.Address;
using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly DapperRepo.DapperRepo _dapper;

        public ProvinceAppService(IRepository<Province, int> repository, DapperRepo.DapperRepo dapper, IGenerateCode generateCode) : base(repository)
        {
            _generateCode = generateCode;
            _dapper = dapper;
        }

        public async override Task<PagedResultDto<ProvinceDto>> GetListAsync(CustomePagedAndSortedResultRequestProvinceDto input)
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

        public async Task<ProvinceDto?> GetProvinceByCode(string code)
        {
            var get = await Repository.FirstOrDefaultAsync(p => p.Code == code);
            if (get == null)
                return null;
            return ObjectMapper.Map<Province, ProvinceDto>(get);
        }
        public async Task<DataResult<ProvinceDto>> ImportExcel(IFormFile formFile)
        {
            
            if (formFile == null || formFile.Length <= 0)
            {
                return DataResult<ProvinceDto>.GetResult(false, "File không hợp lệ hoặc rỗng",null,null);
            }
            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return DataResult<ProvinceDto>.GetResult(false, "Không hỗ trợ định dạng file", null,null);
            }
            var list = new List<ProvinceDto>();
            try
            {
                using (var stream = new MemoryStream())
                {
                    await formFile.CopyToAsync(stream);
                    //ExcelPackage.LicenseContext = LicenseContext.Commercial;
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension.Rows;

                        for (int row = 2; row <= rowCount; row++)
                        {
                            var code = worksheet.Cells[row, 1]?.Value?.ToString()?.Trim();
                            var name = worksheet.Cells[row, 2]?.Value?.ToString()?.Trim();
                            var nameEnglish = worksheet.Cells[row, 3]?.Value?.ToString()?.Trim(); // Có thể rỗng
                            var levelValue = worksheet.Cells[row, 4]?.Value?.ToString()?.Trim();

                            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(name))
                            {
                                continue;
                            }
                            LevelProvince level = LevelProvince.Province;
                            if (!string.IsNullOrEmpty(levelValue))
                            {
                                levelValue = levelValue.ToLower();
                                switch (levelValue)
                                {
                                    case "tỉnh":
                                        level = LevelProvince.Province;
                                        break;
                                    case "thành phố trung ương":
                                        level = LevelProvince.CentralCity;
                                        break;
                                    default:
                                        continue;
                                }
                            }
                            list.Add(new ProvinceDto
                            {
                                Code = code,
                                Name = name,
                                LevelProvince = level
                            });
                        }
                    }
                }
                if (!list.Any())
                {
                    return DataResult<ProvinceDto>.GetResult(false, "Không có dữ liệu hợp lệ để import", null, null);
                }
                // Kiểm tra mã Code trong cơ sở dữ liệu
                var existingCodes = await Repository.GetListAsync(x => list.Select(p => p.Code).Contains(x.Code));
                var validList = new List<ProvinceDto>();
                var errorList = new List<ProvinceDto>();

                foreach (var item in list)
                {
                    if (existingCodes.Any(x => x.Code == item.Code))
                    {
                        errorList.Add(item);
                    }
                    else
                    {
                        validList.Add(item); 
                    }
                }
                if (validList.Any())
                {
                    var entities = validList.Select(item => new Province
                    {
                        Name = item.Name,
                        Code = item.Code,
                        LevelProvince = item.LevelProvince
                    }).ToList();
                    
                    //ObjectMapper.Map<List<ProvinceDto>, List<Province>>(validList);
                    await Repository.InsertManyAsync(entities);
                }
                return DataResult<ProvinceDto>.GetResult(
                                    true,
                                    errorList.Any()
                                        ? $"Import thành công {validList.Count} bản ghi. Có {errorList.Count} bản ghi lỗi."
                                        : "Import thành công tất cả dữ liệu.",
                                    validList,
                                    errorList
                                    );
            }
            catch (Exception ex)
            {
                return DataResult<ProvinceDto>.GetResult(false, $"Đã xảy ra lỗi trong quá trình import: {ex.Message}", null,null);
            }
        }

        public async Task<List<string>> GetAllProvinceCode()
        {
            var query = "SELECT Code FROM Province";
            var list = await _dapper.QueryGetAsync<string>(query);
            return list.ToList();
        }
    }
}
