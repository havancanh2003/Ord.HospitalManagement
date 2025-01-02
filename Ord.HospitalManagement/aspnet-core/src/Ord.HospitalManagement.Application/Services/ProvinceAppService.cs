using AutoMapper;
using AutoMapper.Internal.Mappers;
using IronXL;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using Ord.HospitalManagement.DataResult;
using Ord.HospitalManagement.DomainServices;
using Ord.HospitalManagement.DTOs.Address;
using Ord.HospitalManagement.DTOs.Address.ModelFilter;
using Ord.HospitalManagement.Entities.Address;
using Ord.HospitalManagement.Enums;
using Ord.HospitalManagement.IServices;
using Ord.HospitalManagement.IServices.Address;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace Ord.HospitalManagement.Services
{
    public class ProvinceAppService :
                    CrudAppService<Province, ProvinceDto, int, CustomePagedAndSortedResultRequestProvinceDto, CreateUpdateProvinceDto>,
                    IProvinceAppService
    {
        private readonly IGenerateCode _generateCode;
        //private readonly DapperRepo.DapperRepo _dapper;
        private readonly IDapperRepo _dapper;
        private readonly IMapper _mapper;

        public ProvinceAppService(IRepository<Province, int> repository, IDapperRepo dapper, IMapper mapper, IGenerateCode generateCode) : base(repository)
        {
            _generateCode = generateCode;
            _dapper = dapper;
            _mapper = mapper;
        }

        public async override Task<PagedResultDto<ProvinceDto>> GetListAsync(CustomePagedAndSortedResultRequestProvinceDto input)
        {
            var countQuery = @"SELECT COUNT(*) FROM Province Where 1=1";
            var baseQuery = @"SELECT Id, Code, Name, LevelProvince
                            FROM Province Where 1=1";
            if (!string.IsNullOrEmpty(input.FilterName))
            {
                countQuery += " AND Name LIKE @FilterName";
                baseQuery += " AND Name LIKE @FilterName";
            }

            baseQuery += @" LIMIT @PageSize OFFSET @Offset";
            countQuery += $"; {baseQuery}";
            var parameters = new
            {
                FilterName = $"%{input.FilterName}%",
                Offset = input.SkipCount,
                PageSize = input.MaxResultCount,
            };
            var result = await _dapper.QueryMultiGetAsync<ProvinceDto>(countQuery, parameters);
            return new PagedResultDto<ProvinceDto>(
                result.total,
                result.lists.ToList()
            );
        }

        public override async Task<ProvinceDto> CreateAsync(CreateUpdateProvinceDto input)
        {
            try
            {
                //var province = ObjectMapper.Map<CreateUpdateProvinceDto, Province>(input);
                var province = _mapper.Map<Province>(input);
                province.Code = _generateCode.AutoGenerateCode(PrefixGencode.PrefixGencode.PROV);

                await Repository.InsertAsync(province);

                return  _mapper.Map<ProvinceDto>(province);;
                //return ObjectMapper.Map<Province, ProvinceDto>(province);
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        public override async Task<ProvinceDto> UpdateAsync(int id, CreateUpdateProvinceDto input)
        {
            try
            {
                var existingProvince = await Repository.GetAsync(id);
                if (existingProvince == null) {
                    throw new Exception("Xảy ra lỗi");
                }
                _mapper.Map(input, existingProvince);
                //ObjectMapper.Map(input, existingProvince);
                await Repository.UpdateAsync(existingProvince);

                //return ObjectMapper.Map<Province, ProvinceDto>(existingProvince);
                return _mapper.Map<Province, ProvinceDto>(existingProvince);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public override async Task DeleteAsync(int id)
        {
            try
            {
                if (id == 0)
                {
                    throw new ArgumentException("ID không hợp lệ");
                }
                var existingProvince = await Repository.GetAsync(id);
                if (existingProvince == null)
                {
                   throw new ArgumentException("Xảy ra lỗi");
                }
                await Repository.DeleteAsync(existingProvince);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProvinceDto?> GetProvinceByCode(string code)
        {
            var get = await Repository.FindAsync(p => p.Code == code);
            if (get == null)
                return null;
            //return ObjectMapper.Map<Province, ProvinceDto>(get);
            return _mapper.Map<Province, ProvinceDto>(get);
        }
        public async Task<DataResult<ProvinceDto>> ImportExcel(IFormFile formFile)
        {
            if (formFile == null || formFile.Length <= 0)
            {
                return DataResult<ProvinceDto>.GetResult(false, "File không hợp lệ hoặc rỗng",null,null);
            }
            var extension = Path.GetExtension(formFile.FileName).ToLower();
            if (extension != ".xlsx" && extension != ".xls")
            {
                return DataResult<ProvinceDto>.GetResult(false, "Không hỗ trợ định dạng file", null, null);
            }
            var list = new List<ProvinceDto>();
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
