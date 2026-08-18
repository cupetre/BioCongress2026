namespace Icof.Api.DTOs
{
    public class EventDto
    {
        public Guid Id { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Summary { get; set; }
        public string? Room { get; set; }
        public string? Location { get; set; }

        /// <summary>Serialized enum name, e.g. "Workshop", "Lecture", "Session".</summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>Serialized enum name, e.g. "Open", "Full", "Upcoming".</summary>
        public string Status { get; set; } = string.Empty;

        public DateTimeOffset StartsAtUtc { get; set; }
        public DateTimeOffset? EndsAtUtc { get; set; }
        public bool IsRegistrationEnabled { get; set; }
        public int Capacity { get; set; }
        public int RegisteredCount { get; set; }
        public int DisplayOrder { get; set; }
    }
}
