using AutoMapper;
using Ord.HospitalManagement.DTOs.Address;
using Ord.HospitalManagement.Entities.Address;

namespace Ord.HospitalManagement;

public class HospitalManagementApplicationAutoMapperProfile : Profile
{
    public HospitalManagementApplicationAutoMapperProfile()
    {
        /* configure AutoMapper mapping. */
        CreateMap<Province, ProvinceDto>().ReverseMap();
        CreateMap<CreateUpdateProvinceDto, Province>();

        CreateMap<CreateUpdateDistrictDto, District>();
        CreateMap<District, DistrictDto>().ReverseMap();

        CreateMap<CreateUpdateWardDto, Ward>();
        CreateMap<Ward, WardDto>().ReverseMap();
    }
}
