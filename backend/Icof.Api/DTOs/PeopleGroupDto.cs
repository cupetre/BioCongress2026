namespace Icof.Api.DTOs
{
    public class PeopleGroupDto
    {
        public Guid Id { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }
        public List<TeamMemberDto> Members { get; set; } = new();
    }
}
