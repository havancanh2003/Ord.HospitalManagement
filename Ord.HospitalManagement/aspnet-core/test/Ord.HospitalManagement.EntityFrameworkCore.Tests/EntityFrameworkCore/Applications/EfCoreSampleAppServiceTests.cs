using Ord.HospitalManagement.Samples;
using Xunit;

namespace Ord.HospitalManagement.EntityFrameworkCore.Applications;

[Collection(HospitalManagementTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<HospitalManagementEntityFrameworkCoreTestModule>
{

}
