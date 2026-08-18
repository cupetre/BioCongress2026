namespace Icof.Api.DTOs
{
    /// <summary>
    /// Partial update — any field left null is left unchanged on the existing row. There's
    /// currently no way to clear a field back to null through this endpoint (e.g. remove a
    /// photo); that's a fine limitation for now, revisit if it actually comes up.
    /// </summary>
    public class UpdateTeamMemberRequest
    {
        public string? FullName { get; set; }
        public string? RoleTitle { get; set; }
        public string? Institution { get; set; }
        public string? ShortBio { get; set; }
        public string? Bio { get; set; }
        public string? PhotoBlobName { get; set; }
        public int? DisplayOrder { get; set; }
        public bool? IsPublished { get; set; }
    }
}
