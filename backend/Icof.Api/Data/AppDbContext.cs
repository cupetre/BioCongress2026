using Icof.Api.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Icof.Api.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Event> Events => Set<Event>();
        public DbSet<EventRegistration> EventRegistrations => Set<EventRegistration>();
        public DbSet<TeamMember> TeamMembers => Set<TeamMember>();
        public DbSet<PageContent> PageContents => Set<PageContent>();
        public DbSet<SiteSetting> SiteSettings => Set<SiteSetting>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Event>(entity =>
            {
                entity.ToTable(table =>
                {
                    table.HasCheckConstraint("CK_Events_Capacity_NonNegative", "\"Capacity\" >= 0");
                    table.HasCheckConstraint("CK_Events_RegisteredCount_Valid", "\"RegisteredCount\" >= 0 AND \"RegisteredCount\" <= \"Capacity\"");
                });

                entity.HasIndex(e => e.Slug).IsUnique();
                entity.Property(e => e.Title).HasMaxLength(180).IsRequired();
                entity.Property(e => e.Slug).HasMaxLength(220).IsRequired();
                entity.Property(e => e.Location).HasMaxLength(220);
                entity.Property(e => e.BannerBlobName).HasMaxLength(500);
                entity.Property(e => e.RegisteredCount).HasDefaultValue(0);
                entity.Property(e => e.IsPublished).HasDefaultValue(false);
                entity.Property(e => e.CreatedAtUtc).HasDefaultValueSql("now()");
            });

            builder.Entity<EventRegistration>(entity =>
            {
                entity.HasIndex(r => new { r.UserId, r.EventId }).IsUnique();
                entity.Property(r => r.Status).HasConversion<string>().HasMaxLength(40).IsRequired();
                entity.Property(r => r.RegisteredAtUtc).HasDefaultValueSql("now()");

                entity.HasOne(r => r.Event)
                    .WithMany(e => e.Registrations)
                    .HasForeignKey(r => r.EventId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(r => r.User)
                    .WithMany(u => u.EventRegistrations)
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<TeamMember>(entity =>
            {
                entity.HasIndex(t => t.Slug).IsUnique();
                entity.Property(t => t.FullName).HasMaxLength(180).IsRequired();
                entity.Property(t => t.Slug).HasMaxLength(220).IsRequired();
                entity.Property(t => t.RoleTitle).HasMaxLength(180);
                entity.Property(t => t.PhotoBlobName).HasMaxLength(500);
                entity.Property(t => t.IsPublished).HasDefaultValue(false);
            });

            builder.Entity<PageContent>(entity =>
            {
                entity.HasIndex(p => p.Key).IsUnique();
                entity.Property(p => p.Key).HasMaxLength(120).IsRequired();
                entity.Property(p => p.Title).HasMaxLength(220);
            });

            builder.Entity<SiteSetting>(entity =>
            {
                entity.HasIndex(s => s.Key).IsUnique();
                entity.Property(s => s.Key).HasMaxLength(120).IsRequired();
                entity.Property(s => s.Value).HasMaxLength(2000).IsRequired();
                entity.Property(s => s.ValueType).HasMaxLength(40).IsRequired();
            });
        }
    }
}
