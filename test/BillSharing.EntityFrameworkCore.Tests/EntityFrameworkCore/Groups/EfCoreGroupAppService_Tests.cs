using BillSharing.Groups;
using BillSharing.Samples;
using System;
using Xunit;

namespace BillSharing.EntityFrameworkCore.Groups;

[Collection(BillSharingTestConsts.CollectionDefinitionName)]
public class EfCoreGroupAppService_Tests : GroupAppService_Tests<BillSharingEntityFrameworkCoreTestModule>
{
}
