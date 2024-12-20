using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Ord.HospitalManagement.Data;

/* This is used if database provider does't define
 * IHospitalManagementDbSchemaMigrator implementation.
 */
public class NullHospitalManagementDbSchemaMigrator : IHospitalManagementDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
