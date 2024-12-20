using Ord.HospitalManagement.Samples;
using Xunit;

namespace Ord.HospitalManagement.EntityFrameworkCore.Domains;

[Collection(HospitalManagementTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<HospitalManagementEntityFrameworkCoreTestModule>
{

}
