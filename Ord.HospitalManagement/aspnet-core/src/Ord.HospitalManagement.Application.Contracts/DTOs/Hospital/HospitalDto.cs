using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Ord.HospitalManagement.DTOs.Hospital
{
    public class HospitalDto : AuditedEntityDto<int>
    {
        public string? HospitalName { get; set; }
        public string Hotline { get; set; } = null!;
        public string? Code { get; set; } 
        public string? UserHospitalId { get; set; } 
        public string? ProvinceCode { get; set; } 
        public string? DistrictCode { get; set; } 
        public string? WardCode { get; set; } 
        public string? HospitalDetailAddress { get; set; }
        public string? HospitalDescription { get; set; }
    }
}
