using Icof.Api.Entities;
using Icof.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace Icof.Api.Data
{
    /// <summary>
    /// One-time data seed for PeopleGroups/TeamMembers and Events, run at app startup. Each
    /// section is guarded independently by an "is this table already populated" check, so it's
    /// safe to run every time the app boots (local dotnet run, Docker container, etc.) without
    /// duplicating rows — and adding a new section later won't get skipped just because an
    /// earlier section was already seeded.
    ///
    /// This carries over the same content that used to be hardcoded directly in the Angular
    /// pages (Members/Ambassadors/Workshops/Lectures/Social Programs/Timetable), now living in
    /// the database instead. No photos are seeded — PhotoBlobName/BannerBlobName stay null until
    /// someone uploads one via POST /api/images/..., and the frontend falls back to
    /// initials/placeholders in the meantime, same as before.
    /// </summary>
    public static class DataSeeder
    {
        public static async Task SeedAsync(AppDbContext db, CancellationToken cancellationToken = default)
        {
            await SeedPeopleAsync(db, cancellationToken);
            await SeedEventsAsync(db, cancellationToken);
        }

        private static async Task SeedPeopleAsync(AppDbContext db, CancellationToken cancellationToken)
        {
            if (await db.PeopleGroups.AnyAsync(cancellationToken))
            {
                return; // Already seeded.
            }

            var now = DateTimeOffset.UtcNow;

            PeopleGroup Group(PeopleGroupType type, string name, string? description, int order) => new()
            {
                Id = Guid.NewGuid(),
                Type = type,
                Name = name,
                Slug = SlugHelper.Slugify(name),
                Description = description,
                DisplayOrder = order,
                IsPublished = true,
                CreatedAtUtc = now
            };

            // Fixed, minimal set of real categories — no placeholder people seeded under them.
            // Real people go in one at a time via POST /api/team-members.
            var medicalStudents = Group(PeopleGroupType.MemberGroup, "Medical students", "The medical students behind this year's congress.", 0);
            var professors = Group(PeopleGroupType.MemberGroup, "Professors", "Faculty advisors supporting the congress.", 1);
            var itTeam = Group(PeopleGroupType.MemberGroup, "IT team", "Runs registration systems, the website and on-site technical support.", 2);
            var socialTeam = Group(PeopleGroupType.MemberGroup, "Social team", "Organises the receptions, ceremonies and social side of the congress.", 3);
            var ambassadors = Group(PeopleGroupType.AmbassadorGroup, "Ambassadors", "Our representatives keeping partner faculties connected to ICOF year-round.", 0);

            db.PeopleGroups.AddRange(medicalStudents, professors, itTeam, socialTeam, ambassadors);

            await db.SaveChangesAsync(cancellationToken);
        }

        private static async Task SeedEventsAsync(AppDbContext db, CancellationToken cancellationToken)
        {
            if (await db.Events.AnyAsync(cancellationToken))
            {
                return; // Already seeded.
            }

            var now = DateTimeOffset.UtcNow;
            const string venue = "Faculty of Medicine, Skopje";

            // Congress dates — matches the "Oct 14-16, 2027" date shown on the Timetable page.
            // Times are stored as given (no timezone conversion) — worth revisiting once real
            // scheduling/timezone precision actually matters for registration cutoffs.
            var day1 = new DateTime(2027, 10, 14);
            var day2 = new DateTime(2027, 10, 15);
            var day3 = new DateTime(2027, 10, 16);

            Event Item(
                string title,
                string summary,
                DateTime day,
                string time,
                string room,
                EventType type,
                EventStatus status,
                int capacity,
                int registeredCount,
                bool registrationEnabled,
                int order,
                DateTimeOffset? registrationOpensAtUtc = null)
            {
                var parts = time.Split(':');
                var startsAt = new DateTimeOffset(
                    day.Year, day.Month, day.Day,
                    int.Parse(parts[0]), int.Parse(parts[1]), 0,
                    TimeSpan.Zero);

                return new Event
                {
                    Id = Guid.NewGuid(),
                    Title = title,
                    Slug = SlugHelper.Slugify(title),
                    Summary = summary,
                    Room = room,
                    Location = venue,
                    Type = type,
                    Status = status,
                    StartsAtUtc = startsAt,
                    Capacity = capacity,
                    RegisteredCount = registeredCount,
                    IsRegistrationEnabled = registrationEnabled,
                    RegistrationOpensAtUtc = registrationOpensAtUtc,
                    IsPublished = true,
                    DisplayOrder = order,
                    CreatedAtUtc = now
                };
            }

            var events = new List<Event>
            {
                // Workshops
                Item("Research methods clinic", "Structuring a research abstract from raw data to submission-ready copy. Small group, hands-on.", day1, "11:00", "Room C1", EventType.Workshop, EventStatus.Open, 20, 8, true, 0),
                Item("Suturing & wound closure", "Hands-on surgical skills lab covering basic suturing technique and wound-closure principles.", day1, "14:00", "Room B2", EventType.Workshop, EventStatus.Full, 15, 15, true, 1),
                Item("Point-of-care ultrasound", "An introduction to bedside ultrasound, with supervised scanning practice in small groups.", day2, "14:00", "Room B3", EventType.Workshop, EventStatus.Open, 18, 5, true, 2),
                Item("Emergency simulation", "A simulated trauma scenario in the skills lab, followed by a structured debrief with faculty.", day3, "09:00", "Simulation centre", EventType.Workshop, EventStatus.Upcoming, 20, 0, false, 3, new DateTimeOffset(2027, 6, 1, 0, 0, 0, TimeSpan.Zero)),

                // Lectures
                Item("Opening keynote", "The future of clinical research — an opening address setting the tone for the congress.", day1, "09:30", "Main auditorium", EventType.Lecture, EventStatus.Open, 0, 0, false, 0),
                Item("Research day — abstract presentations", "Student research presented across all tracks, reviewed live by the scientific committee.", day2, "09:00", "Rooms A1, A2, A3", EventType.Lecture, EventStatus.Open, 0, 0, false, 1),
                Item("Cardiology grand round", "A case-based lecture and open discussion led by the cardiology track faculty.", day2, "11:00", "Main auditorium", EventType.Lecture, EventStatus.Open, 0, 0, false, 2),
                Item("Patient lecture", "Living with chronic illness — a patient perspective session on long-term care.", day2, "17:00", "Room A1", EventType.Lecture, EventStatus.Open, 0, 0, false, 3),
                Item("Meet the expert sessions", "Small-group conversations with faculty across specialties — no registration required.", day3, "12:00", "Rooms A1–A3", EventType.Lecture, EventStatus.Open, 0, 0, false, 4),

                // Social programs
                Item("Registration & welcome coffee", "Check in, collect your badge and delegate pack before the congress opens.", day1, "09:00", "Main lobby", EventType.Session, EventStatus.Open, 0, 0, false, 0),
                Item("Welcome reception", "Informal networking for all delegates, faculty and speakers to kick off the congress.", day1, "18:00", "Faculty courtyard", EventType.Session, EventStatus.Open, 0, 0, false, 1),
                Item("Closing keynote & awards", "Congress highlights and the awards ceremony, recognising this year's top abstracts.", day3, "19:00", "Main auditorium", EventType.Session, EventStatus.Open, 0, 0, false, 2),
                Item("Closing gala", "An evening of celebration to close the congress — dinner, music and one last chance to connect.", day3, "20:30", "Faculty courtyard", EventType.Session, EventStatus.Open, 0, 0, false, 3)
            };

            db.Events.AddRange(events);

            await db.SaveChangesAsync(cancellationToken);
        }
    }
}
