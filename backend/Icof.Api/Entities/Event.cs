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
        public string? BannerBlobName { get; set; }
        public DateTimeOffset StartsAtUtc { get; set; }
        public DateTimeOffset? EndsAtUtc { get; set; }
        public DateTimeOffset? RegistrationOpensAtUtc { get; set; }
        public DateTimeOffset? RegistrationClosesAtUtc { get; set; }
        public int Capacity { get; set; }
        public int RegisteredCount { get; set; }
        public bool IsPublished { get; set; }
        public DateTimeOffset CreatedAtUtc { get; set; }
        public DateTimeOffset? UpdatedAtUtc { get; set; }

        public ICollection<EventRegistration> Registrations { get; set; } = new List<EventRegistration>();
    }
}
