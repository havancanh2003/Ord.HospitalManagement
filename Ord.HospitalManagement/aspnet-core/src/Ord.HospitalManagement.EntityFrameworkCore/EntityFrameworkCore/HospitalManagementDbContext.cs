using Microsoft.EntityFrameworkCore;
using Ord.HospitalManagement.Entities;
using Ord.HospitalManagement.Entities.Address;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace Ord.HospitalManagement.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class HospitalManagementDbContext :
    AbpDbContext<HospitalManagementDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    //Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }
    // Entity create 
    public DbSet<Province> Provinces { get; set; }
    public DbSet<District> Districts { get;set; }
    public DbSet<Ward> Wards { get; set; }
    public DbSet<Hospital> Hospitals { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<UserHospital> UserHospitals { get; set; }


    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    public HospitalManagementDbContext(DbContextOptions<HospitalManagementDbContext> options)
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
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        /* Configure your own tables/entities inside here */

        builder.Entity<Province>(b =>
        {
            b.ToTable("Province");
            b.ConfigureByConvention();
            b.Property(x => x.Code).IsRequired().HasMaxLength(128);
            b.HasIndex(x => x.Code).IsUnique();
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
        });
        builder.Entity<District>(b =>
        {
            b.ToTable("District");
            b.ConfigureByConvention();
            b.Property(x => x.Code).IsRequired().HasMaxLength(128);
            b.HasIndex(x => x.Code).IsUnique();
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            b.Property(x => x.ProvinceCode).IsRequired();
        });
        builder.Entity<Ward>(b =>
        {
            b.ToTable("Ward");
            b.ConfigureByConvention();
            b.Property(x => x.Code).IsRequired().HasMaxLength(128);
            b.HasIndex(x => x.Code).IsUnique();
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            b.Property(x => x.ProvinceCode).IsRequired();
            b.Property(x => x.DistrictCode).IsRequired();
        });

        builder.Entity<Patient>(b =>
        {
            b.ToTable("Patient");
            b.ConfigureByConvention();
            b.Property(x => x.Fullname).IsRequired().HasMaxLength(128);
            b.Property(x => x.HospitalId).IsRequired();
            b.HasIndex(x => x.Code).IsUnique();
            b.Property(x => x.ProvinceCode).IsRequired();
            b.Property(x => x.DistrictCode).IsRequired();
            b.Property(x => x.WardCode).IsRequired();
        });
        builder.Entity<Hospital>(b =>
        {
            b.ToTable("Hospital");
            b.ConfigureByConvention();
            b.Property(x => x.HospitalName).IsRequired().HasMaxLength(128);
            b.Property(x => x.ProvinceCode).IsRequired();
            b.HasIndex(x => x.Code).IsUnique();
            b.Property(x => x.DistrictCode).IsRequired();
            b.Property(x => x.WardCode).IsRequired();
        });
        builder.Entity<UserHospital>(b =>
        {
            b.ToTable("UserHospital");
            b.ConfigureByConvention();
            b.Property(x => x.HospitalId).IsUnicode();
            b.Property(x => x.UserId).IsUnicode();
        });
    }
}
