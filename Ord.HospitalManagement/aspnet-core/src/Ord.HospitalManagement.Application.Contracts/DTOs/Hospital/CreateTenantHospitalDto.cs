using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ord.HospitalManagement.DTOs.Hospital
{
    public class CreateTenantHospitalDto
    {
        public CreateTenantHospitalDto()
        {
            userHospital = new CreateUserHospital();
            createHospital = new CreateUpdateHospitalDto();
        }
        public CreateUserHospital userHospital { get; set; }
        public CreateUpdateHospitalDto createHospital { get; set; }

    }
}
