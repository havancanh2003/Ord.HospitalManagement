using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Ord.HospitalManagement.IServices.Hospital
{
    public interface IUserHospitalSerivice : IApplicationService
    {
        Task<int?> GetHospitalId(Guid? currenAdminId);
    }
}
