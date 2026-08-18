namespace Icof.Api.DTOs
{
    public class CreateTeamMemberRequest
    {
        /// <summary>Either this or PeopleGroupId must be set — whichever is easier to hand.</summary>
        public string? PeopleGroupSlug { get; set; }
        public Guid? PeopleGroupId { get; set; }

        public string FullName { get; set; } = string.Empty;
        public string? RoleTitle { get; set; }
        public string? Institution { get; set; }
        public string? ShortBio { get; set; }
        public string? Bio { get; set; }

        /// <summary>
        /// The blob name (not full URL) of a photo already uploaded via POST /api/images/people
        /// or directly through the Azure Portal — e.g. "people/eva-ristovska.png". Optional;
        /// leave null and the frontend falls back to showing an initial.
        /// </summary>
        public string? PhotoBlobName { get; set; }

        /// <summary>Leave null to auto-append at the end of the group.</summary>
        public int? DisplayOrder { get; set; }
    }
}
