using AutoMapper;
using Ord.HospitalManagement.DTOs.Address;
using Ord.HospitalManagement.DTOs.Hospital;
using Ord.HospitalManagement.Entities;
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

        CreateMap<CreateUpdateHospitalDto, Hospital>();
        CreateMap<Hospital, HospitalDto>().ReverseMap();

        CreateMap<CreateUpdatePatientDto, Patient>();
        CreateMap<Patient, PatientDto>().ReverseMap();
    }
}
