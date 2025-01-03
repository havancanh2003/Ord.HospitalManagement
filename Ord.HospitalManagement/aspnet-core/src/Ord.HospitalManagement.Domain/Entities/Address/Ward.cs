﻿using Ord.HospitalManagement.Enums;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ord.HospitalManagement.Entities.Address
{
    public class Ward : AuditedAggregateRoot<int>
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public LevelWard LevelWard { get; set; } = LevelWard.Ward;
        public string DistrictCode { get; set; } = null!;
        public string ProvinceCode { get; set; } = null!;
    }
}
