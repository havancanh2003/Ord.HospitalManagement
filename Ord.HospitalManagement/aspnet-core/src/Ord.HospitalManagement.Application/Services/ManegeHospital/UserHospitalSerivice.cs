using Ord.HospitalManagement.Entities;
using Ord.HospitalManagement.IServices.Hospital;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Ord.HospitalManagement.Services.ManegeHospital
{
    public class UserHospitalSerivice : ApplicationService, IUserHospitalSerivice, IScopedDependency
    {
        private readonly IRepository<UserHospital, int> _repository;
        public UserHospitalSerivice(IRepository<UserHospital, int> repository)
        {
            _repository = repository;
        }
        public async Task<int?> GetHospitalId(Guid? currenAdminId)
        {
            var ush = await _repository.FirstOrDefaultAsync(uh => uh.UserId.Equals(currenAdminId));
            if (ush != null) 
                return ush.HospitalId; ;

            return null;
        }
    }
}
