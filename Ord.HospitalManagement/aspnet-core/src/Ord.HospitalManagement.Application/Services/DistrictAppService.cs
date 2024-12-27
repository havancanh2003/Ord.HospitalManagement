using IronXL;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
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
    public class DistrictAppService : CrudAppService<District, DistrictDto, int, CustomePagedAndSortedResultRequestDistrictDto, CreateUpdateDistrictDto>, IDistrictAppService
    {
        private readonly IGenerateCode _generateCode;
        private readonly IProvinceAppService _provinceAppService;
        private readonly DapperRepo.DapperRepo _dapper;
        public DistrictAppService(IRepository<District, int> repository, IGenerateCode generateCode, DapperRepo.DapperRepo dapper, IProvinceAppService provinceAppService) : base(repository)
        {
            _generateCode = generateCode;
            _provinceAppService = provinceAppService;
            _dapper = dapper;
        }

        public async override Task<PagedResultDto<DistrictDto>> GetListAsync(CustomePagedAndSortedResultRequestDistrictDto input)
        {
            var countQuery = @"SELECT COUNT(*) FROM District Where 1=1";
            var baseQuery = @"SELECT Id, Code, Name, LevelDistrict, ProvinceCode 
                            FROM District Where 1=1";
            if (!string.IsNullOrEmpty(input.ProvinceCode))
            {
                countQuery += " AND ProvinceCode = @ProvinceCode";
                baseQuery += " AND ProvinceCode = @ProvinceCode";
            }
            if (!string.IsNullOrEmpty(input.FilterName))
            {
                countQuery += " AND Name LIKE @FilterName";
                baseQuery += " AND Name LIKE @FilterName";
            }

            baseQuery += @" LIMIT @PageSize OFFSET @Offset";
            countQuery += $"; {baseQuery}";
            var parameters = new
            {
                ProvinceCode = input.ProvinceCode,
                FilterName = $"%{input.FilterName}%",
                Offset = input.SkipCount,
                PageSize = input.MaxResultCount,
            };
            var result = await _dapper.QueryMultiGetAsync<DistrictDto>(countQuery, parameters);
            return new PagedResultDto<DistrictDto>(
                result.total,
                result.lists.ToList()
            );
        }
        /// <summary>
        /// Overide
        /// </summary>
        public override async Task<DistrictDto> CreateAsync(CreateUpdateDistrictDto input)
        {
            if (string.IsNullOrEmpty(input.ProvinceCode))
                throw new Exception();

            var checkProvince = await _provinceAppService.GetProvinceByCode(input.ProvinceCode);
            if (checkProvince == null)
                throw new Exception();

            var district = ObjectMapper.Map<CreateUpdateDistrictDto, District>(input);
            district.Code = _generateCode.AutoGenerateCode(PrefixGencode.PrefixGencode.DIST);

            await Repository.InsertAsync(district);
            return ObjectMapper.Map<District, DistrictDto>(district);
        }
        /// <summary>
        /// Overide
        /// </summary>
        public override async Task<DistrictDto> UpdateAsync(int id, CreateUpdateDistrictDto input)
        {
            if (string.IsNullOrEmpty(input.ProvinceCode))
                throw new Exception();

            var checkProvince = await _provinceAppService.GetProvinceByCode(input.ProvinceCode);
            if (checkProvince == null)
                throw new Exception();

            var existingDistrict = await Repository.GetAsync(id);
            if (existingDistrict == null)
                throw new Exception();

            ObjectMapper.Map(input, existingDistrict);
            await Repository.UpdateAsync(existingDistrict);

            return ObjectMapper.Map<District, DistrictDto>(existingDistrict);
        }

        public async Task<DistrictDto?> GetDistrictByCode(string code)
        {
            var get = await Repository.FirstOrDefaultAsync(p => p.Code == code);
            if (get == null)
                return null;
            return ObjectMapper.Map<District, DistrictDto>(get);
        }
        public async Task<List<ModelDistrictCodeProvinCodeMap>> GetAllDistrictCode()
        {
            var query = "SELECT Code AS DistrictCode,ProvinceCode FROM District";
            var list = await _dapper.QueryGetAsync<ModelDistrictCodeProvinCodeMap>(query);
            return list.ToList();
        }

        public async Task<DataResult<DistrictDto>> ImportExcel(IFormFile formFile)
        {
            if (formFile == null || formFile.Length <= 0)
            {
                return DataResult<DistrictDto>.GetResult(false, "File không hợp lệ hoặc rỗng", null, null);
            }
            var extension = Path.GetExtension(formFile.FileName).ToLower();
            if (extension != ".xlsx" && extension != ".xls")
            {
                return DataResult<DistrictDto>.GetResult(false, "Không hỗ trợ định dạng file", null, null);
            }
            var list = new List<DistrictDto>();
            try
            {
                using (var stream = new MemoryStream())
                {
                    await formFile.CopyToAsync(stream);
                    stream.Position = 0;

                    MemoryStream processedStream = new MemoryStream();

                    if (extension == ".xls")
                    {
                        var xlsWorkbook = WorkBook.Load(stream);
                        var tempFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
                        xlsWorkbook.SaveAs(tempFilePath);
                        processedStream = new MemoryStream(File.ReadAllBytes(tempFilePath));
                        File.Delete(tempFilePath);
                    }
                    else
                    {
                        stream.CopyTo(processedStream);
                    }
                    processedStream.Position = 0;
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(processedStream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension.Rows;

                        for (int row = 2; row <= rowCount; row++)
                        {
                            var provinceCode = worksheet.Cells[row, 2]?.Value?.ToString()?.Trim();
                            var name = worksheet.Cells[row, 3]?.Value?.ToString()?.Trim();
                            var code = worksheet.Cells[row, 4]?.Value?.ToString()?.Trim();
                            var levelValue = worksheet.Cells[row, 5]?.Value?.ToString()?.Trim();

                            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(provinceCode))
                            {
                                continue;
                            }
                            LevelDistrict level = LevelDistrict.District;
                            if (!string.IsNullOrEmpty(levelValue))
                            {
                                levelValue = levelValue.ToLower();
                                switch (levelValue)
                                {
                                    case "huyện":
                                        level = LevelDistrict.District;
                                        break;
                                    case "thành phố":
                                        level = LevelDistrict.City;
                                        break;
                                    case "quận":
                                        level = LevelDistrict.DistrictCity;
                                        break;
                                    case "thị xã":
                                        level = LevelDistrict.Town;
                                        break;
                                    default:
                                        continue;
                                }
                            }
                            list.Add(new DistrictDto
                            {
                                Code = code,
                                Name = name,
                                ProvinceCode = provinceCode,
                                LevelDistrict = level
                            });
                        }
                    }
                }
                if (!list.Any())
                {
                    return DataResult<DistrictDto>.GetResult(false, "Không có dữ liệu hợp lệ để import", null, null);
                }
                // Kiểm tra mã Code trong cơ sở dữ liệu
                var existingCodes = await Repository.GetListAsync(x => list.Select(p => p.Code).Contains(x.Code));
                var existingProvinceCodes = await _provinceAppService.GetAllProvinceCode();
                var validList = new List<DistrictDto>();
                var errorList = new List<DistrictDto>();

                foreach (var item in list)
                {
                    if (existingCodes.Any(x => x.Code == item.Code) || !existingProvinceCodes.Contains(item.ProvinceCode))
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
                    var entities = validList.Select(item => new District
                    {
                        Name = item.Name,
                        Code = item.Code,
                        ProvinceCode = item.ProvinceCode,
                        LevelDistrict = item.LevelDistrict
                    }).ToList();

                    //ObjectMapper.Map<List<ProvinceDto>, List<Province>>(validList);
                    await Repository.InsertManyAsync(entities);
                }
                return DataResult<DistrictDto>.GetResult(
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
                return DataResult<DistrictDto>.GetResult(false, $"Đã xảy ra lỗi trong quá trình import: {ex.Message}", null, null);
            }
        }
    }
}
