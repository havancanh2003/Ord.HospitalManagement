using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace Ord.HospitalManagement.Entities.Address
{
    public class Province : Entity<int>
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}
