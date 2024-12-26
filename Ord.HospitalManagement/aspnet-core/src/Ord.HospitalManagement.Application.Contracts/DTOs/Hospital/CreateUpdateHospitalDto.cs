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
        [Required(ErrorMessage = "Hospital Name là trường bắt buộc.")]
        [MaxLength(100, ErrorMessage = "Hospital Name là trường bắt buộc(<100)")]
        public string HospitalName { get; set; } = null!;
        public string ProvinceCode { get; set; } = null!;
        public string DistrictCode { get; set; } = null!;
        public string WardCode { get; set; } = null!;
        public string? HospitalDetailAddress { get; set; }
        public string? HospitalDescription { get; set; }
        [Required(ErrorMessage = "Hotline là trường bắt buộc.")]
        [Phone(ErrorMessage = "Hotline không đúng tiêu chuẩn phone number.")]
        [MaxLength(15, ErrorMessage = "Hotline không quá 15 kí tự.")]
        public string Hotline { get; set; } = null!;
    }
}
