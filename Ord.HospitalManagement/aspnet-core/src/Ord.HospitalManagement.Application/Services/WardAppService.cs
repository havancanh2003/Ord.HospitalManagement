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
            try
            {
                if (string.IsNullOrEmpty(input.ProvinceCode))
                    throw new ArgumentNullException();

                if (string.IsNullOrEmpty(input.DistrictCode))
                    throw new ArgumentNullException();

                var checkProvince = await _provinceAppService.GetProvinceByCode(input.ProvinceCode);
                if (checkProvince == null)
                    throw new Exception("Không tồn tại tỉnh trong hệ thống");

                var checkDistrict = await _districtAppService.GetDistrictByCode(input.DistrictCode);
                if (checkDistrict == null)
                    throw new Exception("Không tồn tại huyện trong hệ thống");

                var ward = ObjectMapper.Map<CreateUpdateWardDto, Ward>(input);
                ward.Code = _generateCode.AutoGenerateCode(PrefixGencode.PrefixGencode.WARD);

                await Repository.InsertAsync(ward);
                return ObjectMapper.Map<Ward, WardDto>(ward);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public override async Task<WardDto> UpdateAsync(int id, CreateUpdateWardDto input)
        {
            try
            {
                if (string.IsNullOrEmpty(input.ProvinceCode))
                    throw new ArgumentNullException();

                if (string.IsNullOrEmpty(input.DistrictCode))
                    throw new ArgumentNullException();

                var checkProvince = await _provinceAppService.GetProvinceByCode(input.ProvinceCode);
                if (checkProvince == null)
                    throw new Exception("Không tồn tại tỉnh trong hệ thống");

                var checkDistrict = await _districtAppService.GetDistrictByCode(input.DistrictCode);
                if (checkDistrict == null)
                    throw new Exception("Không tồn tại huyện trong hệ thống");

                var existingWard = await Repository.GetAsync(id);
                if (existingWard == null)
                    throw new Exception("Không tồn tại xã trong hệ thống");

                ObjectMapper.Map(input, existingWard);
                await Repository.UpdateAsync(existingWard);

                return ObjectMapper.Map<Ward, WardDto>(existingWard);

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public async Task<DataResult<WardDto>> ImportExcelWard(IFormFile formFile)
        {
            if (formFile == null || formFile.Length <= 0)
            {
                return DataResult<WardDto>.GetResult(false, "File không hợp lệ hoặc rỗng", null, null);
            }
            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return DataResult<WardDto>.GetResult(false, "Không hỗ trợ định dạng file", null, null);
            }
            var list = new List<WardDto>();
            try
            {
                using (var stream = new MemoryStream())
                {
                    await formFile.CopyToAsync(stream);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension.Rows;

                        for (int row = 2; row <= rowCount; row++)
                        {
                            var provinceName = worksheet.Cells[row, 1]?.Value?.ToString()?.Trim();
                            var provinceCode = worksheet.Cells[row, 2]?.Value?.ToString()?.Trim();
                            var districtName = worksheet.Cells[row, 3]?.Value?.ToString()?.Trim();
                            var districtCode = worksheet.Cells[row, 4]?.Value?.ToString()?.Trim();
                            var wardName = worksheet.Cells[row, 5]?.Value?.ToString()?.Trim();
                            var wardCode = worksheet.Cells[row, 6]?.Value?.ToString()?.Trim();
                            var levelValue = worksheet.Cells[row, 7]?.Value?.ToString()?.Trim();
                            //var englishName = worksheet.Cells[row, 8]?.Value?.ToString()?.Trim();

                            if (string.IsNullOrEmpty(wardCode) || string.IsNullOrEmpty(wardName) || string.IsNullOrEmpty(districtCode) || string.IsNullOrEmpty(provinceCode))
                            {
                                continue;
                            }
                            LevelWard level = LevelWard.Commune;
                            if (!string.IsNullOrEmpty(levelValue))
                            {
                                levelValue = levelValue.ToLower();
                                switch (levelValue)
                                {
                                    case "phường":
                                        level = LevelWard.Ward;
                                        break;
                                    case "xã":
                                        level = LevelWard.Commune;
                                        break;
                                    case "thị trấn":
                                        level = LevelWard.Township;
                                        break;
                                    default:
                                        continue;
                                }
                            }
                            list.Add(new WardDto
                            {
                                ProvinceCode = provinceCode,
                                DistrictCode = districtCode,
                                Name = wardName,
                                Code = wardCode,
                                LevelWard = level,
                            });
                        }
                    }
                }
                if (!list.Any())
                {
                    return DataResult<WardDto>.GetResult(false, "Không có dữ liệu hợp lệ để import", null, null);
                }
                var existingWardCodes = await Repository.GetListAsync(x => list.Select(p => p.Code).Contains(x.Code));
                // Lấy danh sách huyện với tỉnh tương ứng(ở đây get ra cả mã huyện và mã tỉnh tương ứng của huyện đó)
                var allDistricts = await _districtAppService.GetAllDistrictCode();
                // lấy 2 làm key
                var validDistrictProvincePairs =  allDistricts.ToDictionary(d => d.DistrictCode, d => d.ProvinceCode);

                var validList = new List<WardDto>();
                var errorList = new List<WardDto>();

                foreach (var item in list)
                {
                    // Kiểm tra mã huyện và tỉnh tồn tại trong hệ thống và có khớp nhau ko
                    // TH cả mã tỉnh và huyện đều cùng tồn tại, nhưng huyện đó lại ko thuộc tỉnh này => ko nhất quán về dữ liệu
                    if (!validDistrictProvincePairs.TryGetValue(item.DistrictCode, out var associatedProvinceCode) ||
                        associatedProvinceCode != item.ProvinceCode || existingWardCodes.Any(x => x.Code == item.Code))
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
                    var entities = validList.Select(item => new Ward
                    {
                        ProvinceCode = item.ProvinceCode,
                        DistrictCode = item.DistrictCode,
                        Name = item.Name,
                        Code = item.Code,
                        LevelWard = item.LevelWard,
                    }).ToList();

                    await Repository.InsertManyAsync(entities);
                }

                return DataResult<WardDto>.GetResult(
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
                return DataResult<WardDto>.GetResult(false, $"Đã xảy ra lỗi trong quá trình import: {ex.Message}", null, null);
            }
        }

    }
}
