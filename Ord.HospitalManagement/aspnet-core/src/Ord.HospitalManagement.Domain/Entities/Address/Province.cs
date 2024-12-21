using Ord.HospitalManagement.Enums;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ord.HospitalManagement.Entities.Address
{
    public class Province : AuditedAggregateRoot<int>
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public LevelProvince LevelProvince { get; set; } = LevelProvince.Province;
    }
}
