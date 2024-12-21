using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ord.HospitalManagement.DomainServices
{
    public interface IGenerateCode
    {
        string AutoGenerateCode(string prefix);
    }
}
