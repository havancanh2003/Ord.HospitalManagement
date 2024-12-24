using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ord.HospitalManagement.Entities
{
    public class Hospital : AuditedAggregateRoot<int>
    {
        public string HospitalName { get; set; } = null!;
        public string Hotline { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string ProvinceCode { get; set; } = null!;
        public string DistrictCode { get; set; } = null!;
        public string WardCode { get; set; } = null!;
        public string? HospitalDetailAddress { get; set; }
        public string? HospitalDescription { get; set; }
    }
}
