namespace Icof.Api.DTOs
{
    public class TeamMemberDto
    {
        public Guid Id { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? RoleTitle { get; set; }
        public string? Institution { get; set; }
        public string? ShortBio { get; set; }
        public string? Bio { get; set; }

        /// <summary>
        /// Resolved from PhotoBlobName server-side. Null if no photo has been uploaded yet —
        /// the frontend falls back to an initial/placeholder in that case.
        /// </summary>
        public string? PhotoUrl { get; set; }

        public int DisplayOrder { get; set; }
    }
}
