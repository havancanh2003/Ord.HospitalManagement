using Ord.HospitalManagement.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Ord.HospitalManagement.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(HospitalManagementEntityFrameworkCoreModule),
    typeof(HospitalManagementApplicationContractsModule)
    )]
public class HospitalManagementDbMigratorModule : AbpModule
{
}
