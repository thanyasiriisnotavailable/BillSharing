using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace BillSharing.Pages;

[Collection(BillSharingTestConsts.CollectionDefinitionName)]
public class Index_Tests : BillSharingWebTestBase
{
    [Fact]
    public async Task Welcome_Page()
    {
        var response = await GetResponseAsStringAsync("/");
        response.ShouldNotBeNull();
    }
}
