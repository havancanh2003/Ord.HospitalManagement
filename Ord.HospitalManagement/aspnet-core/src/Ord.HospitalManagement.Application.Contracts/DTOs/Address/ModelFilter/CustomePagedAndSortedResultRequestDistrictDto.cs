using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Ord.HospitalManagement.DTOs.Address.ModelFilter
{
    public class CustomePagedAndSortedResultRequestDistrictDto : PagedAndSortedResultRequestDto
    {
        public string? ProvinceCode { get; set; }
        public string? FilterName { get; set; }
    }
}
