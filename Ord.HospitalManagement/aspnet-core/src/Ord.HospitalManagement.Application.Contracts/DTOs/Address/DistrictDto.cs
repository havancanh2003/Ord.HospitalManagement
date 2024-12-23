using Ord.HospitalManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace Ord.HospitalManagement.DTOs.Address
{
    public class DistrictDto : AuditedEntityDto<int>
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string ProvinceCode { get; set; } = null!;
        public LevelDistrict LevelDistrict { get; set; }
    }
}
