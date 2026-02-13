using Volo.Abp.Modularity;

namespace BillSharing;

/* Inherit from this class for your domain layer tests. */
public abstract class BillSharingDomainTestBase<TStartupModule> : BillSharingTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
