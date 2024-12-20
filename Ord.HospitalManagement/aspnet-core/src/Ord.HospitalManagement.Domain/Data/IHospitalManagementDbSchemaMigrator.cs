using System.Threading.Tasks;

namespace Ord.HospitalManagement.Data;

public interface IHospitalManagementDbSchemaMigrator
{
    Task MigrateAsync();
}
