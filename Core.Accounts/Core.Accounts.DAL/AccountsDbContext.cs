using Common;
using Common.Interface;
using Common.Model;
using Core.Accounts.DAL.Entity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Core.Accounts.DAL
{
    public class AccountsDbContext : BaseDbContext
    {
        public AccountsDbContext(DbContextOptions<AccountsDbContext> options)
             : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<AccountRole> AccountRoles { get; set; }

        public AuthenticatedAccount CurrentAccount { get; set; } = new AuthenticatedAccount { Id = 1 };
        public string Token { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                        .HasIndex(u => u.Email)
                        .IsUnique();

            modelBuilder.Entity<AccountRole>()
            .HasIndex(a => new { a.AccountId, a.RoleId })
                    .IsUnique(true);

            modelBuilder.Entity<Role>()
                        .HasIndex(u => new { u.Id, u.TenatId })
                        .IsUnique(true);

            modelBuilder.Entity<Account>()
                        .ToTable("Accounts", b => b.IsTemporal());

            modelBuilder.Entity<AccountRole>()
              .ToTable("AccountRoles", b => b.IsTemporal());

            modelBuilder.Entity<Role>()
              .ToTable("Roles", b => b.IsTemporal());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.LogTo(message => Debug.WriteLine(message));
        // optionsBuilder.LogTo(Console.WriteLine);

    }
}
