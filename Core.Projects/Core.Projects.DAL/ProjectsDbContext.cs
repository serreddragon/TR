using Core.Projects.DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace Core.Projects.DAL
{
    public class ProjectsDbContext : DbContext
    {
        public ProjectsDbContext(DbContextOptions<ProjectsDbContext> options)
            : base(options) { }

        // -------- DbSets

        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectPhase> ProjectPhases { get; set; }
        public DbSet<ActivityGroup> ActivityGroups { get; set; }
        public DbSet<Activity> Activities { get; set; }

        // -------- Override Methodes

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>()
                .HasIndex(t => t.Id)
                .IsUnique(true);

            modelBuilder.Entity<ProjectPhase>()
                .HasIndex(t => t.Id)
                .IsUnique(true);

            modelBuilder.Entity<ActivityGroup>()
                .HasIndex(t => t.Id)
                .IsUnique(true);

            modelBuilder.Entity<Activity>()
                .HasIndex(t => t.Id)
                .IsUnique(true);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-5NVMTPB;Database=TR.Core.Projects;Integrated Security=SSPI;");
            }
        }

    }
}
