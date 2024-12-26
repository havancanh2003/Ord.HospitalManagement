using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ord.HospitalManagement.DTOs
{
    public class ManageInfoHospital
    {
        public int HospitalId { get; set; } 
        public string? HospitalName { get; set; }
        public string? HospitalCode { get; set; }
        public string? ProvinceCode { get; set; }
        public string? DistrictCode { get; set; }
        public string? WardCode { get; set; }
        public string? DetailAddress { get; set; }
        public string? AdminHospitalName { get; set; }
        public string? HospitalDescription { get; set; }
        public string? Hotline { get; set; }
        public Guid UserId { get; set; }
    }
}
