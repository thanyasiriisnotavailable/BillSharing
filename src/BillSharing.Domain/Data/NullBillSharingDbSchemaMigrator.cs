using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace BillSharing.Data;

/* This is used if database provider does't define
 * IBillSharingDbSchemaMigrator implementation.
 */
public class NullBillSharingDbSchemaMigrator : IBillSharingDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
