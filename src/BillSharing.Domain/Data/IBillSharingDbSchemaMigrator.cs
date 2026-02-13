using System.Threading.Tasks;

namespace BillSharing.Data;

public interface IBillSharingDbSchemaMigrator
{
    Task MigrateAsync();
}
