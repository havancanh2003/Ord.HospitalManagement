using Ord.HospitalManagement.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ord.HospitalManagement.Entities.Address
{
    public class District : AuditedAggregateRoot<int>
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string ProvinceCode { get; set; } = null!;
        public LevelDistrict LevelDistrict { get; set; } = LevelDistrict.District;

    }
}
