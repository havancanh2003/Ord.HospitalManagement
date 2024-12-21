using Ord.HospitalManagement.Enums;

namespace Ord.HospitalManagement.DTOs.Address
{
    public class DistrictDto
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string ProvinceCode { get; set; } = null!;
        public LevelDistrict LevelDistrict { get; set; }
    }
}
