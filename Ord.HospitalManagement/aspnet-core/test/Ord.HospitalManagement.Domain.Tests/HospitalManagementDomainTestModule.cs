using Volo.Abp.Modularity;

namespace Ord.HospitalManagement;

[DependsOn(
    typeof(HospitalManagementDomainModule),
    typeof(HospitalManagementTestBaseModule)
)]
public class HospitalManagementDomainTestModule : AbpModule
{

}
