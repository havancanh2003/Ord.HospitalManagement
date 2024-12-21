using Ord.HospitalManagement.Enums;
using System.ComponentModel.DataAnnotations;

namespace Ord.HospitalManagement.DTOs.Address
{
    public class CreateUpdateProvinceDto
    {
        [Required]
        [StringLength(128)] 
        public string Name { get; set; } = null!;
        [Required]
        public LevelProvince LevelProvince { get; set; }
    }
}
