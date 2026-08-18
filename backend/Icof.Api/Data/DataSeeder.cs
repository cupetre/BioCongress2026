using System.Text;
using Icof.Api.Entities;
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
                Slug = Slugify(name),
                Description = description,
                DisplayOrder = order,
                IsPublished = true,
                CreatedAtUtc = now
            };

            TeamMember Member(string fullName, string? roleTitle, string? bio, int order) => new()
            {
                Id = Guid.NewGuid(),
                FullName = fullName,
                Slug = Slugify(fullName),
                RoleTitle = roleTitle,
                Bio = bio,
                DisplayOrder = order,
                IsPublished = true,
                CreatedAtUtc = now
            };

            var studentTeam = Group(
                PeopleGroupType.MemberGroup,
                "Student medical team",
                "The students organising this year's congress — programme, logistics, communications and delegate care.",
                0);
            studentTeam.Members.Add(Member("Marija Stojanova", "Student medical team", "4th year medicine — leads delegate communications and the daily on-site schedule.", 0));
            studentTeam.Members.Add(Member("Petar Angelov", "Student medical team", "6th year medicine — coordinates volunteers and room logistics across all three days.", 1));
            studentTeam.Members.Add(Member("Ivana Trpkova", "Student medical team", "3rd year medicine — manages registration desk and delegate check-in.", 2));
            studentTeam.Members.Add(Member("Aleksandar Nikolov", "Student medical team", "5th year medicine — runs the workshop equipment and technical setup.", 3));

            var professors = Group(
                PeopleGroupType.MemberGroup,
                "Professors",
                "Faculty advisors who guide the academic direction of the congress and support the scientific programme.",
                1);
            professors.Members.Add(Member("Prof. Biljana Trajkova", "Faculty advisor", "Senior lecturer in cardiology and long-standing academic advisor to ICOF.", 0));
            professors.Members.Add(Member("Prof. Goran Miloševski", "Faculty advisor", "Dean's office liaison, oversees faculty-level approvals and venue access.", 1));
            professors.Members.Add(Member("Prof. Ivan Cvetanovski", "Research committee", "Chairs the abstract review board for the annual research day.", 2));

            var scientificTeam = Group(
                PeopleGroupType.MemberGroup,
                "Scientific team",
                "Reviews abstracts, builds the academic programme and briefs speakers ahead of each session.",
                2);
            scientificTeam.Members.Add(Member("Dr. Elena Georgieva", "Cardiology track lead", "Reviews cardiology abstracts and chairs the cardiology session.", 0));
            scientificTeam.Members.Add(Member("Dr. Filip Ristovski", "Neurology track lead", "Coordinates the neurology lecture block and speaker briefings.", 1));
            scientificTeam.Members.Add(Member("Dr. Sara Kovačevska", "Public health track lead", "Oversees the public health and research day submissions.", 2));

            var financeIt = Group(
                PeopleGroupType.MemberGroup,
                "Finance & IT",
                "Keeps the congress funded, budgeted and technically running — from sponsorship invoicing to the website itself.",
                3);
            financeIt.Members.Add(Member("Bojan Stefanovski", "Finance lead", "Manages the congress budget, invoicing and sponsor payments.", 0));
            financeIt.Members.Add(Member("Kristina Naumova", "IT & systems", "Runs registration systems, the website and on-site technical support.", 1));

            var contributors = Group(
                PeopleGroupType.MemberGroup,
                "Main contributors",
                "Long-standing volunteers and former organisers who continue to support ICOF year over year.",
                4);
            contributors.Members.Add(Member("Ana Petrovska", "President", "Final-year medical student leading this year's organising committee.", 0));
            contributors.Members.Add(Member("Darko Ilievski", "Scientific committee", "Oversees abstract review and the research day programme.", 1));
            contributors.Members.Add(Member("Nina Đorđević", "Alumni advisor", "Former president, now advising the current committee.", 2));

            var ambassadors = Group(
                PeopleGroupType.AmbassadorGroup,
                "International Ambassadors",
                "Our international representatives — one per partner faculty, keeping their home institution connected to ICOF year-round.",
                0);
            ambassadors.Members.Add(Member("Elena Petkovska", "North Macedonia", "Coordinates the host-faculty delegation and helps first-time delegates find their way around the venue.", 0));
            ambassadors.Members.Add(Member("Marko Jovanović", "Serbia", "Promotes ICOF at the Belgrade Faculty of Medicine and organises the travelling delegate group.", 1));
            ambassadors.Members.Add(Member("Yana Dimitrova", "Bulgaria", "Runs delegate recruitment in Sofia and Plovdiv, and liaises with the scientific committee on abstracts.", 2));
            ambassadors.Members.Add(Member("Dimitris Papadopoulos", "Greece", "Builds partnerships with Greek medical faculties and coordinates joint research submissions.", 3));
            ambassadors.Members.Add(Member("Erisa Hoxha", "Albania", "Leads outreach in Tirana and supports Albanian delegates with travel and accommodation questions.", 4));
            ambassadors.Members.Add(Member("Blerta Krasniqi", "Kosovo", "Coordinates the Pristina delegate group and represents ICOF at regional student conferences.", 5));
            ambassadors.Members.Add(Member("Andrei Popescu", "Romania", "Organises the Bucharest and Cluj delegations and helps first-year students prepare their first abstract.", 6));
            ambassadors.Members.Add(Member("Ivana Kovač", "Croatia", "Runs the Zagreb ambassador network and coordinates joint workshops with visiting faculty.", 7));

            db.PeopleGroups.AddRange(studentTeam, professors, scientificTeam, financeIt, contributors, ambassadors);

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
                    Slug = Slugify(title),
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

        private static string Slugify(string value)
        {
            var normalized = value
                .Replace("č", "c").Replace("Č", "C")
                .Replace("ć", "c").Replace("Ć", "C")
                .Replace("š", "s").Replace("Š", "S")
                .Replace("ž", "z").Replace("Ž", "Z")
                .Replace("đ", "dj").Replace("Đ", "Dj");

            var builder = new StringBuilder();
            var lastWasHyphen = false;

            foreach (var c in normalized.ToLowerInvariant())
            {
                if (char.IsLetterOrDigit(c))
                {
                    builder.Append(c);
                    lastWasHyphen = false;
                }
                else if (!lastWasHyphen && builder.Length > 0)
                {
                    builder.Append('-');
                    lastWasHyphen = true;
                }
            }

            return builder.ToString().TrimEnd('-');
        }
    }
}
