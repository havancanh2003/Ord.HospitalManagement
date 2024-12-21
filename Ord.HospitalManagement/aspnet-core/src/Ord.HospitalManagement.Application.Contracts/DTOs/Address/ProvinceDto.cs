using Ord.HospitalManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace Ord.HospitalManagement.DTOs.Address
{
    public class ProvinceDto : AuditedEntityDto<int>
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public LevelProvince LevelProvince { get; set; }
    }
}
