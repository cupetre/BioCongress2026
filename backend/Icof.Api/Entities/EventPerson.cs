namespace Icof.Api.Entities
{
    public class EventPerson
    {
        public Guid EventId { get; set; }
        public Guid TeamMemberId { get; set; }
        public EventPersonRole Role { get; set; } = EventPersonRole.Speaker;
        public int DisplayOrder { get; set; }

        public Event Event { get; set; } = null!;
        public TeamMember TeamMember { get; set; } = null!;
    }

    public enum EventPersonRole
    {
        Speaker,
        Presenter,
        Moderator,
        Guest,
        Participant
    }
}
