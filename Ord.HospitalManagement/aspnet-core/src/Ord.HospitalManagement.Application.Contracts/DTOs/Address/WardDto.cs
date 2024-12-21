using Ord.HospitalManagement.Enums;

namespace Ord.HospitalManagement.DTOs.Address
{
    public class WardDto
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public LevelWard LevelWard { get; set; }
        public string DistrictCode { get; set; } = null!;
        public string ProvinceCode { get; set; } = null!;
    }
}
