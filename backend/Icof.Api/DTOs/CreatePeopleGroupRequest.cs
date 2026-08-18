using Icof.Api.Entities;

namespace Icof.Api.DTOs
{
    public class CreatePeopleGroupRequest
    {
        public PeopleGroupType Type { get; set; } = PeopleGroupType.MemberGroup;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        /// <summary>Leave null to auto-append at the end of the existing groups of this Type.</summary>
        public int? DisplayOrder { get; set; }
    }
}
