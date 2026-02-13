using Volo.Abp.Modularity;

namespace BillSharing;

[DependsOn(
    typeof(BillSharingDomainModule),
    typeof(BillSharingTestBaseModule)
)]
public class BillSharingDomainTestModule : AbpModule
{

}
