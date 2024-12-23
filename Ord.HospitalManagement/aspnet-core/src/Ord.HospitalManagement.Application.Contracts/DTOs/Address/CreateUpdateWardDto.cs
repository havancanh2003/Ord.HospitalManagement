using Ord.HospitalManagement.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ord.HospitalManagement.DTOs.Address
{
    public class CreateUpdateWardDto
    {
        [Required]
        [StringLength(128)]
        public string Name { get; set; } = null!;
        [Required]
        public LevelWard LevelWard { get; set; }
        [Required]
        public string ProvinceCode { get; set; } = null!;
        [Required]
        public string DistrictCode { get; set; } = null!;
    }
}
