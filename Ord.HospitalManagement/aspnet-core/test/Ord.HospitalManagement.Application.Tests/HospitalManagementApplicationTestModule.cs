using Volo.Abp.Modularity;

namespace Ord.HospitalManagement;

[DependsOn(
    typeof(HospitalManagementApplicationModule),
    typeof(HospitalManagementDomainTestModule)
)]
public class HospitalManagementApplicationTestModule : AbpModule
{

}
