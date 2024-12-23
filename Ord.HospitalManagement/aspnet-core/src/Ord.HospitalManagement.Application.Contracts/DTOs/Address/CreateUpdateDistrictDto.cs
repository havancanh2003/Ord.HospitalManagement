using Ord.HospitalManagement.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ord.HospitalManagement.DTOs.Address
{
    public class CreateUpdateDistrictDto
    {
        [Required]
        [StringLength(128)]
        public string Name { get; set; } = null!;
        [Required]
        public LevelDistrict LevelDistrict { get; set; }
        [Required]
        public string ProvinceCode { get; set; } = null!;

    }
}
