using Volo.Abp.Modularity;

namespace BillSharing;

[DependsOn(
    typeof(BillSharingApplicationModule),
    typeof(BillSharingDomainTestModule)
)]
public class BillSharingApplicationTestModule : AbpModule
{

}
