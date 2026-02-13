using BillSharing.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace BillSharing.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(BillSharingEntityFrameworkCoreModule),
    typeof(BillSharingApplicationContractsModule)
)]
public class BillSharingDbMigratorModule : AbpModule
{
}
