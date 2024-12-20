using Volo.Abp.Modularity;

namespace Ord.HospitalManagement;

/* Inherit from this class for your domain layer tests. */
public abstract class HospitalManagementDomainTestBase<TStartupModule> : HospitalManagementTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
