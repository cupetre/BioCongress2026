using Icof.Api.Entities;

namespace Icof.Api.DTOs
{
    /// <summary>Partial update — any field left null is left unchanged on the existing row.</summary>
    public class UpdateEventRequest
    {
        public string? Title { get; set; }
        public string? Summary { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public string? Room { get; set; }
        public EventType? Type { get; set; }
        public EventStatus? Status { get; set; }

        public DateTimeOffset? StartsAtUtc { get; set; }
        public DateTimeOffset? EndsAtUtc { get; set; }
        public DateTimeOffset? RegistrationOpensAtUtc { get; set; }
        public DateTimeOffset? RegistrationClosesAtUtc { get; set; }

        public int? Capacity { get; set; }
        public int? RegisteredCount { get; set; }
        public bool? IsRegistrationEnabled { get; set; }
        public string? EligibilityNotes { get; set; }
        public string? RegistrationCtaLabel { get; set; }

        public int? DisplayOrder { get; set; }
        public bool? IsPublished { get; set; }
    }
}
