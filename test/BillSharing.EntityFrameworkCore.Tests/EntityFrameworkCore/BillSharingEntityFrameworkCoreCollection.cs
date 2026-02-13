using Xunit;

namespace BillSharing.EntityFrameworkCore;

[CollectionDefinition(BillSharingTestConsts.CollectionDefinitionName)]
public class BillSharingEntityFrameworkCoreCollection : ICollectionFixture<BillSharingEntityFrameworkCoreFixture>
{

}
