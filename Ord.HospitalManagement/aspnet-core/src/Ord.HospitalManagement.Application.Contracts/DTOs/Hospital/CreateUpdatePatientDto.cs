using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ord.HospitalManagement.DTOs.Hospital
{
    public class CreateUpdatePatientDto
    {
        [Required(ErrorMessage ="Tên bệnh nhân không được để trống")]
        public string Fullname { get; set; } = null!;
        public string ProvinceCode { get; set; } = null!;
        public string DistrictCode { get; set; } = null!;
        public string WardCode { get; set; } = null!;
        public string? DetailAddress { get; set; }
        public DateTime? Birthday { get; set; }
        public string? MedicalHistory { get; set; }
    }
}
