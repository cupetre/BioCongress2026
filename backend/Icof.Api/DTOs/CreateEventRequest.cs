using Icof.Api.Entities;

namespace Icof.Api.DTOs
{
    public class CreateEventRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Summary { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public string? Room { get; set; }
        public EventType Type { get; set; } = EventType.Session;
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

        /// <summary>Leave null to auto-append at the end of the existing events of this Type.</summary>
        public int? DisplayOrder { get; set; }
    }
}
