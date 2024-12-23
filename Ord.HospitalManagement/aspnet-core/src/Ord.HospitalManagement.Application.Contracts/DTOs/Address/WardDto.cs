using Ord.HospitalManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace Ord.HospitalManagement.DTOs.Address
{
    public class WardDto : AuditedEntityDto<int>
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public LevelWard LevelWard { get; set; }
        public string DistrictCode { get; set; } = null!;
        public string ProvinceCode { get; set; } = null!;
    }
}
