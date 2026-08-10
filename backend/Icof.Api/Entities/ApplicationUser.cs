using Microsoft.AspNetCore.Identity;

namespace Icof.Api.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTimeOffset CreatedAtUtc { get; set; }
        public DateTimeOffset? LastLoginAtUtc { get; set; }

        public ICollection<EventRegistration> EventRegistrations { get; set; } = new List<EventRegistration>();
    }
}
