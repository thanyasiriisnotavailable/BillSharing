using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using BillSharing.Groups;
using BillSharing.Expenses;

namespace BillSharing.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ConnectionStringName("Default")]
public class BillSharingDbContext :
    AbpDbContext<BillSharingDbContext>,
    IIdentityDbContext
{
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupMember> GroupMembers { get; set; }

    public DbSet<Expense> Expenses { get; set; }
    public DbSet<ExpenseItem> ExpenseItems { get; set; }
    public DbSet<ItemSplit> ItemSplits { get; set; }



    #region Entities from the modules

    /* Notice: We only implemented IIdentityProDbContext 
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityProDbContext .
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    // Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }

    #endregion

    public BillSharingDbContext(DbContextOptions<BillSharingDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureFeatureManagement();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureBlobStoring();

        builder.Entity<Group>(b =>
        {
            b.ToTable(
                BillSharingConsts.DbTablePrefix + "Groups",
                BillSharingConsts.DbSchema
            );

            b.ConfigureByConvention();

            b.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(128);

            b.Property(x => x.Description)
                .HasMaxLength(512);

            b.Property(x => x.InviteCode)
                .IsRequired()
                .HasMaxLength(16);

            b.HasIndex(x => x.InviteCode)
                .IsUnique();

            // Group → Members
            b.HasMany(x => x.Members)
                .WithOne(x => x.Group)
                .HasForeignKey(x => x.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            // Group → Expenses
            b.HasMany(x => x.Expenses)
                .WithOne()
                .HasForeignKey(x => x.GroupId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<GroupMember>(b =>
        {
            b.ToTable(
                BillSharingConsts.DbTablePrefix + "GroupMembers",
                BillSharingConsts.DbSchema
            );

            b.ConfigureByConvention();

            b.Property(x => x.JoinedAt)
                .IsRequired();

            // Prevent duplicate member in same group
            b.HasIndex(x => new { x.GroupId, x.UserId })
                .IsUnique();

            b.HasOne(x => x.Group)
                .WithMany(x => x.Members)
                .HasForeignKey(x => x.GroupId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Expense>(b =>
        {
            b.ToTable(
                BillSharingConsts.DbTablePrefix + "Expenses",
                BillSharingConsts.DbSchema
            );

            b.ConfigureByConvention();

            b.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(256);

            b.Property(x => x.ExpenseDate)
                .IsRequired();

            b.HasMany(x => x.Items)
                .WithOne(x => x.Expense)
                .HasForeignKey(x => x.ExpenseId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<ExpenseItem>(b =>
        {
            b.ToTable(
                BillSharingConsts.DbTablePrefix + "ExpenseItems",
                BillSharingConsts.DbSchema
            );

            b.ConfigureByConvention();

            b.Property(x => x.ItemName)
                .IsRequired()
                .HasMaxLength(256);

            b.Property(x => x.TotalAmount)
                .HasColumnType("decimal(18,2)");

            b.HasMany(x => x.Splits)
                .WithOne(x => x.ExpenseItem)
                .HasForeignKey(x => x.ExpenseItemId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<ItemSplit>(b =>
        {
            b.ToTable(
                BillSharingConsts.DbTablePrefix + "ItemSplits",
                BillSharingConsts.DbSchema
            );

            b.ConfigureByConvention();

            b.Property(x => x.ShareAmount)
                .HasColumnType("decimal(18,2)");

            b.Property(x => x.IsPaid)
                .IsRequired();

            b.HasIndex(x => new { x.ExpenseItemId, x.UserId });

            b.HasOne(x => x.ExpenseItem)
                .WithMany(x => x.Splits)
                .HasForeignKey(x => x.ExpenseItemId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
