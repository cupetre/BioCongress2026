namespace Icof.Api.Entities
{
    public class TeamMember
    {
        public Guid Id { get; set; }
        public Guid? PeopleGroupId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? RoleTitle { get; set; }
        public string? Institution { get; set; }
        public string? Specialty { get; set; }
        public string? ShortBio { get; set; }
        public string? Bio { get; set; }
        public string? PhotoBlobName { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsPublished { get; set; }
        public DateTimeOffset CreatedAtUtc { get; set; }
        public DateTimeOffset? UpdatedAtUtc { get; set; }

        public PeopleGroup? PeopleGroup { get; set; }
        public ICollection<EventPerson> Events { get; set; } = new List<EventPerson>();
    }
}
