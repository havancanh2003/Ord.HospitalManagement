using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ord.HospitalManagement.Entities
{
    public class UserHospital : AuditedAggregateRoot<int>
    {
        public Guid UserId { get; set; }
        public int HospitalId { get; set; }
    }
}
