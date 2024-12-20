using Xunit;

namespace Ord.HospitalManagement.EntityFrameworkCore;

[CollectionDefinition(HospitalManagementTestConsts.CollectionDefinitionName)]
public class HospitalManagementEntityFrameworkCoreCollection : ICollectionFixture<HospitalManagementEntityFrameworkCoreFixture>
{

}
