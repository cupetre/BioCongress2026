namespace Icof.Api.Entities
{
    public class Organization
    {
        public Guid Id { get; set; }
        public OrganizationType Type { get; set; } = OrganizationType.Partner;
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? LogoBlobName { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsPublished { get; set; }
        public DateTimeOffset CreatedAtUtc { get; set; }
        public DateTimeOffset? UpdatedAtUtc { get; set; }
    }

    public enum OrganizationType
    {
        Partner,
        Sponsor,
        SupportingInstitution
    }
}
