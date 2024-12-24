using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ord.HospitalManagement.Entities
{
    public class Patient : AuditedAggregateRoot<int>
    {
        public int HospitalId { get; set; }
        public string Code { get; set; } = null!;
        public string Fullname { get; set; } = null!;
        public string ProvinceCode { get; set; } = null!;
        public string DistrictCode { get; set; } = null!;
        public string WardCode { get; set; } = null!;
        public string? DetailAddress { get; set; }
        public DateTime? Birthday { get; set; }
        public string? MedicalHistory { get; set; }
    }
}
