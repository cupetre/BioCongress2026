namespace Icof.Api.Services
{
    public interface IEventRegistrationService
    {
        Task<EventRegistrationResult> RegisterAsync(Guid eventId, string userId, CancellationToken cancellationToken);
    }

    public record EventRegistrationResult(EventRegistrationResultCode Code, string Message)
    {
        public bool Succeeded => Code == EventRegistrationResultCode.Registered;
    }

    public enum EventRegistrationResultCode
    {
        Registered,
        AlreadyRegistered,
        EventNotFound,
        RegistrationClosed,
        EventFull
    }
}
