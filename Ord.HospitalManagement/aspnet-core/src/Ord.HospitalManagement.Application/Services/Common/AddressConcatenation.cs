using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;

namespace Ord.HospitalManagement.Services.Common
{
    public class AddressConcatenation : ApplicationService,IScopedDependency
    {
        private readonly DapperRepo.DapperRepo _dapperRepo;
        public AddressConcatenation(DapperRepo.DapperRepo dapperRepo)
        {
            _dapperRepo = dapperRepo;
        }
        //public async Task<string> DetailAddress(string? pCode, string? dCode, string? wCode, string? detail)
        //{
        //    var result = new List<string>();
        //    if (!string.IsNullOrWhiteSpace(detail))
        //    {
        //        result.Add(detail);
        //    }
        //    if (!string.IsNullOrWhiteSpace(wCode))
        //    {
        //        var query = "SELECT Name FROM Ward WHERE Code = @Code";
        //        var wardName = await _dapperRepo.QuerySingleAsync<string>(query, new { Code = wCode });
        //        if (!string.IsNullOrWhiteSpace(wardName))
        //        {
        //            result.Add(wardName);
        //        }
        //    }
        //    if (!string.IsNullOrWhiteSpace(dCode))
        //    {
        //        var query = "SELECT Name FROM District WHERE Code = @Code";
        //        var districtName = await _dapperRepo.QuerySingleAsync<string>(query, new { Code = dCode });
        //        if (!string.IsNullOrWhiteSpace(districtName))
        //        {
        //            result.Add(districtName);
        //        }
        //    }
        //    if (!string.IsNullOrWhiteSpace(pCode))
        //    {
        //        var query = "SELECT Name FROM Province WHERE Code = @Code";
        //        var provinceName = await _dapperRepo.QuerySingleAsync<string>(query, new { Code = pCode });
        //        if (!string.IsNullOrWhiteSpace(provinceName))
        //        {
        //            result.Add(provinceName);
        //        }
        //    }
        //    return string.Join(", ", result);
        //}
        public async Task<string> DetailAddress(string? pCode, string? dCode, string? wCode, string? detail)
        {
            var result = new List<string>();

            if (!string.IsNullOrWhiteSpace(detail))
            {
                result.Add(detail);
            }

            var query = @"
                        SELECT 
                            w.Name AS WardName,
                            d.Name AS DistrictName,
                            p.Name AS ProvinceName
                        FROM Ward w
                        LEFT JOIN District d ON d.Code = @DistrictCode
                        LEFT JOIN Province p ON p.Code = @ProvinceCode
                        WHERE w.Code = @WardCode";

            var parameters = new { WardCode = wCode, DistrictCode = dCode, ProvinceCode = pCode };
            var addressData = await _dapperRepo.QuerySingleAsync<dynamic>(query, parameters);

            if (!string.IsNullOrWhiteSpace(addressData?.WardName))
            {
                result.Add(addressData.WardName);
            }

            if (!string.IsNullOrWhiteSpace(addressData?.DistrictName))
            {
                result.Add(addressData.DistrictName);
            }

            if (!string.IsNullOrWhiteSpace(addressData?.ProvinceName))
            {
                result.Add(addressData.ProvinceName);
            }

            return string.Join(", ", result);
        }

    }
}
