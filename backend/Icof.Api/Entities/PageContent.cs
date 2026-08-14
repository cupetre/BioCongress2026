namespace Icof.Api.Entities
{
    public class PageContent
    {
        public Guid Id { get; set; }
        public string Key { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public PageSection Section { get; set; } = PageSection.Icof;
        public string? Title { get; set; }
        public string? Summary { get; set; }
        public string HtmlContent { get; set; } = string.Empty;
        public string? HeroBlobName { get; set; }
        public string? MetaDescription { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsPublished { get; set; }
        public DateTimeOffset CreatedAtUtc { get; set; }
        public DateTimeOffset? UpdatedAtUtc { get; set; }
        public string? UpdatedByUserId { get; set; }
    }

    public enum PageSection
    {
        Icof,
        Programme,
        Participation,
        Contact,
        Home
    }
}
