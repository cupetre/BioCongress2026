using Icof.Api.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Icof.Api.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Event> Events => Set<Event>();
        public DbSet<EventAgendaItem> EventAgendaItems => Set<EventAgendaItem>();
        public DbSet<EventPerson> EventPeople => Set<EventPerson>();
        public DbSet<EventRegistration> EventRegistrations => Set<EventRegistration>();
        public DbSet<Organization> Organizations => Set<Organization>();
        public DbSet<PeopleGroup> PeopleGroups => Set<PeopleGroup>();
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
                entity.Property(e => e.Room).HasMaxLength(120);
                entity.Property(e => e.BannerBlobName).HasMaxLength(500);
                entity.Property(e => e.Type).HasConversion<string>().HasMaxLength(40).HasDefaultValue(EventType.Congress).IsRequired();
                entity.Property(e => e.Status).HasConversion<string>().HasMaxLength(40).HasDefaultValue(EventStatus.Draft).IsRequired();
                entity.Property(e => e.RegistrationCtaLabel).HasMaxLength(80);
                entity.Property(e => e.RegisteredCount).HasDefaultValue(0);
                entity.Property(e => e.IsRegistrationEnabled).HasDefaultValue(false);
                entity.Property(e => e.IsPublished).HasDefaultValue(false);
                entity.Property(e => e.CreatedAtUtc).HasDefaultValueSql("now()");
            });

            builder.Entity<EventAgendaItem>(entity =>
            {
                entity.Property(a => a.Title).HasMaxLength(180).IsRequired();

                entity.HasOne(a => a.Event)
                    .WithMany(e => e.AgendaItems)
                    .HasForeignKey(a => a.EventId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<EventPerson>(entity =>
            {
                entity.HasKey(ep => new { ep.EventId, ep.TeamMemberId, ep.Role });
                entity.Property(ep => ep.Role).HasConversion<string>().HasMaxLength(40).IsRequired();

                entity.HasOne(ep => ep.Event)
                    .WithMany(e => e.People)
                    .HasForeignKey(ep => ep.EventId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ep => ep.TeamMember)
                    .WithMany(t => t.Events)
                    .HasForeignKey(ep => ep.TeamMemberId)
                    .OnDelete(DeleteBehavior.Cascade);
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
                entity.Property(t => t.Institution).HasMaxLength(220);
                entity.Property(t => t.Specialty).HasMaxLength(180);
                entity.Property(t => t.PhotoBlobName).HasMaxLength(500);
                entity.Property(t => t.IsPublished).HasDefaultValue(false);

                entity.HasOne(t => t.PeopleGroup)
                    .WithMany(g => g.Members)
                    .HasForeignKey(t => t.PeopleGroupId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            builder.Entity<PeopleGroup>(entity =>
            {
                entity.HasIndex(g => g.Slug).IsUnique();
                entity.Property(g => g.Type).HasConversion<string>().HasMaxLength(40).IsRequired();
                entity.Property(g => g.Name).HasMaxLength(180).IsRequired();
                entity.Property(g => g.Slug).HasMaxLength(220).IsRequired();
                entity.Property(g => g.HeroBlobName).HasMaxLength(500);
                entity.Property(g => g.IsPublished).HasDefaultValue(false);
                entity.Property(g => g.CreatedAtUtc).HasDefaultValueSql("now()");
            });

            builder.Entity<Organization>(entity =>
            {
                entity.HasIndex(o => o.Slug).IsUnique();
                entity.Property(o => o.Type).HasConversion<string>().HasMaxLength(40).IsRequired();
                entity.Property(o => o.Name).HasMaxLength(180).IsRequired();
                entity.Property(o => o.Slug).HasMaxLength(220).IsRequired();
                entity.Property(o => o.WebsiteUrl).HasMaxLength(500);
                entity.Property(o => o.LogoBlobName).HasMaxLength(500);
                entity.Property(o => o.IsPublished).HasDefaultValue(false);
                entity.Property(o => o.CreatedAtUtc).HasDefaultValueSql("now()");
            });

            builder.Entity<PageContent>(entity =>
            {
                entity.HasIndex(p => p.Key).IsUnique();
                entity.HasIndex(p => p.Slug).IsUnique();
                entity.Property(p => p.Key).HasMaxLength(120).IsRequired();
                entity.Property(p => p.Slug).HasMaxLength(220).IsRequired();
                entity.Property(p => p.Section).HasConversion<string>().HasMaxLength(40).HasDefaultValue(PageSection.Icof).IsRequired();
                entity.Property(p => p.Title).HasMaxLength(220);
                entity.Property(p => p.HeroBlobName).HasMaxLength(500);
                entity.Property(p => p.MetaDescription).HasMaxLength(320);
                entity.Property(p => p.IsPublished).HasDefaultValue(false);
                entity.Property(p => p.CreatedAtUtc).HasDefaultValueSql("now()");
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
