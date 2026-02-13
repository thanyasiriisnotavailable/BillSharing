using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BillSharing.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class BillSharingDbContextFactory : IDesignTimeDbContextFactory<BillSharingDbContext>
{
    public BillSharingDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        
        BillSharingEfCoreEntityExtensionMappings.Configure();

        var builder = new DbContextOptionsBuilder<BillSharingDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));
        
        return new BillSharingDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../BillSharing.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables();

        return builder.Build();
    }
}
