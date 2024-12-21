using AutoMapper;
using Ord.HospitalManagement.DTOs.Address;
using Ord.HospitalManagement.Entities.Address;

namespace Ord.HospitalManagement;

public class HospitalManagementApplicationAutoMapperProfile : Profile
{
    public HospitalManagementApplicationAutoMapperProfile()
    {
        /* configure AutoMapper mapping. */
        CreateMap<Province, ProvinceDto>();
        CreateMap<CreateUpdateProvinceDto, Province>();
        CreateMap<District, DistrictDto>();
        CreateMap<Ward, WardDto>();
    }
}
