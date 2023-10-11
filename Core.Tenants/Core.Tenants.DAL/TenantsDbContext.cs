using Common;
using Core.Tenants.DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace Core.Tenants.DAL
{
    public class TenantsDbContext : BaseDbContext
    {
        public TenantsDbContext(DbContextOptions<TenantsDbContext> options)
      : base(options) { }

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<AccountTenantMembership> AccountTenantMemberships { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tenant>()
                .HasIndex(t => t.Id)
                .IsUnique(true);

            modelBuilder.Entity<AccountTenantMembership>()
                    .HasIndex(t => t.Id)
                    .IsUnique(true);

            modelBuilder.Entity<AccountTenantMembership>().
                    HasIndex(a => new { a.AccountId, a.TenantId })
                    .IsUnique(true);

            modelBuilder.Entity<Tenant>().ToTable("Tenants", t => t.IsTemporal());

            modelBuilder.Entity<AccountTenantMembership>().ToTable("AccountTenantMembership", t => t.IsTemporal());
        }
    }
}