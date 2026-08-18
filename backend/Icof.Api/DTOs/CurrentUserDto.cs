namespace Icof.Api.DTOs
{
    public class CurrentUserDto
    {
        public string Email { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();
    }
}
