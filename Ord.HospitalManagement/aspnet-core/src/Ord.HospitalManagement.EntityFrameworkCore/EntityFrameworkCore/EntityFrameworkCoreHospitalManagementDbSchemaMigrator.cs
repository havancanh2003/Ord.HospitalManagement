using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ord.HospitalManagement.Data;
using Volo.Abp.DependencyInjection;

namespace Ord.HospitalManagement.EntityFrameworkCore;

public class EntityFrameworkCoreHospitalManagementDbSchemaMigrator
    : IHospitalManagementDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreHospitalManagementDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the HospitalManagementDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<HospitalManagementDbContext>()
            .Database
            .MigrateAsync();
    }
}
