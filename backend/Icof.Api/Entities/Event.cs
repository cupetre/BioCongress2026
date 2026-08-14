namespace Icof.Api.Entities
{
    public class Event
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Summary { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public string? Room { get; set; }
        public string? BannerBlobName { get; set; }
        public EventType Type { get; set; } = EventType.Congress;
        public EventStatus Status { get; set; } = EventStatus.Draft;
        public DateTimeOffset StartsAtUtc { get; set; }
        public DateTimeOffset? EndsAtUtc { get; set; }
        public DateTimeOffset? RegistrationOpensAtUtc { get; set; }
        public DateTimeOffset? RegistrationClosesAtUtc { get; set; }
        public int Capacity { get; set; }
        public int RegisteredCount { get; set; }
        public bool IsRegistrationEnabled { get; set; }
        public string? EligibilityNotes { get; set; }
        public string? RegistrationCtaLabel { get; set; }
        public bool IsPublished { get; set; }
        public int DisplayOrder { get; set; }
        public DateTimeOffset CreatedAtUtc { get; set; }
        public DateTimeOffset? UpdatedAtUtc { get; set; }

        public ICollection<EventRegistration> Registrations { get; set; } = new List<EventRegistration>();
        public ICollection<EventAgendaItem> AgendaItems { get; set; } = new List<EventAgendaItem>();
        public ICollection<EventPerson> People { get; set; } = new List<EventPerson>();
    }

    public enum EventType
    {
        Congress,
        Workshop,
        Lecture,
        Session
    }

    public enum EventStatus
    {
        Draft,
        Upcoming,
        Open,
        Closed,
        Full,
        Completed,
        Cancelled
    }
}
