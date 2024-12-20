using Volo.Abp.Modularity;

namespace Ord.HospitalManagement;

public abstract class HospitalManagementApplicationTestBase<TStartupModule> : HospitalManagementTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
