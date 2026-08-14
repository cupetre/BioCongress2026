namespace Icof.Api.Entities
{
    public class PeopleGroup
    {
        public Guid Id { get; set; }
        public PeopleGroupType Type { get; set; } = PeopleGroupType.MemberGroup;
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? HeroBlobName { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsPublished { get; set; }
        public DateTimeOffset CreatedAtUtc { get; set; }
        public DateTimeOffset? UpdatedAtUtc { get; set; }

        public ICollection<TeamMember> Members { get; set; } = new List<TeamMember>();
    }

    public enum PeopleGroupType
    {
        AmbassadorGroup,
        MemberGroup,
        ContributorGroup
    }
}
