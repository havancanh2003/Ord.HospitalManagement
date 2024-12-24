using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ord.HospitalManagement.DTOs.Hospital
{
    public class CreateUpdateHospitalDto 
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Hospital Name cannot exceed 100 characters.")]
        public string HospitalName { get; set; } = null!;
        public string ProvinceCode { get; set; } = null!;
        public string DistrictCode { get; set; } = null!;
        public string WardCode { get; set; } = null!;
        public string? HospitalDetailAddress { get; set; }
        public string? HospitalDescription { get; set; }
        [Required(ErrorMessage = "Hotline is required.")]
        [Phone(ErrorMessage = "Hotline must be a valid phone number.")]
        [MaxLength(15, ErrorMessage = "Hotline cannot exceed 15 characters.")]
        public string Hotline { get; set; } = null!;
    }
}
