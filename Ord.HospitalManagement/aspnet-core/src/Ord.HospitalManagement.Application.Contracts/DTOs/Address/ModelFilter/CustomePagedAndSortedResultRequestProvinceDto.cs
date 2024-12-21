
using Volo.Abp.Application.Dtos;

namespace Ord.HospitalManagement.DTOs.Address.ModelFilter
{
    public class CustomePagedAndSortedResultRequestProvinceDto : PagedAndSortedResultRequestDto
    {
        public string? FilterName { get; set; }
    }
}
