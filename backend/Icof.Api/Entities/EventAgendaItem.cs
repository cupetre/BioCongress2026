namespace Icof.Api.Entities
{
    public class EventAgendaItem
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTimeOffset? StartsAtUtc { get; set; }
        public DateTimeOffset? EndsAtUtc { get; set; }
        public int DisplayOrder { get; set; }

        public Event Event { get; set; } = null!;
    }
}
