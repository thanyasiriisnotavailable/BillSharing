using BillSharing.Samples;
using Xunit;

namespace BillSharing.EntityFrameworkCore.Applications;

[Collection(BillSharingTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<BillSharingEntityFrameworkCoreTestModule>
{

}
