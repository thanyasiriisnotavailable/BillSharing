using Volo.Abp.Modularity;

namespace BillSharing;

public abstract class BillSharingApplicationTestBase<TStartupModule> : BillSharingTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
