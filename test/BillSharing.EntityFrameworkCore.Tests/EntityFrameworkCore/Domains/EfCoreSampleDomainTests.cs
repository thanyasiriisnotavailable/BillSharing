using BillSharing.Samples;
using Xunit;

namespace BillSharing.EntityFrameworkCore.Domains;

[Collection(BillSharingTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<BillSharingEntityFrameworkCoreTestModule>
{

}
