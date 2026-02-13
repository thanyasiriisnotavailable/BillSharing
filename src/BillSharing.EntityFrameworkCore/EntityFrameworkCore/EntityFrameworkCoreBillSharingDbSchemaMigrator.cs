using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BillSharing.Data;
using Volo.Abp.DependencyInjection;

namespace BillSharing.EntityFrameworkCore;

public class EntityFrameworkCoreBillSharingDbSchemaMigrator
    : IBillSharingDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreBillSharingDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the BillSharingDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<BillSharingDbContext>()
            .Database
            .MigrateAsync();
    }
}
