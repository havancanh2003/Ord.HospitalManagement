using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.MultiTenancy;

namespace Ord.HospitalManagement.DTOs.Hospital
{
    public class PatientDto : AuditedEntityDto<int>
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
