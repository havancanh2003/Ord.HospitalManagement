using System;
using Volo.Abp.DependencyInjection;
namespace Ord.HospitalManagement.DomainServices
{
    [ExposeServices(typeof(IGenerateCode))]
    public class GenerateCode : IGenerateCode, ITransientDependency
    {
        public string AutoGenerateCode(string prefix)
        {
            if(string.IsNullOrEmpty(prefix)) 
                return string.Empty;

            int randomNumber = new Random().Next(10, 10000); 
            return $"{prefix}_{randomNumber}";
        }
    }
}
