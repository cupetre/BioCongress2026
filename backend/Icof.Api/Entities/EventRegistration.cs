namespace Icof.Api.Entities
{
    public class EventRegistration
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public RegistrationStatus Status { get; set; } = RegistrationStatus.Confirmed;
        public DateTimeOffset RegisteredAtUtc { get; set; }
        public DateTimeOffset? CancelledAtUtc { get; set; }

        public Event Event { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
    }

    public enum RegistrationStatus
    {
        Confirmed,
        Cancelled
    }
}
