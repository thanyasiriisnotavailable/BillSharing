using Microsoft.AspNetCore.Builder;
using BillSharing;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
builder.Environment.ContentRootPath = GetWebProjectContentRootPathHelper.Get("BillSharing.Web.csproj"); 
await builder.RunAbpModuleAsync<BillSharingWebTestModule>(applicationName: "BillSharing.Web");

public partial class Program
{
}
