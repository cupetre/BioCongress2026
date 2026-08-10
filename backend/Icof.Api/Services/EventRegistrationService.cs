using System.Data;
using Icof.Api.Data;
using Icof.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Icof.Api.Services
{
    public class EventRegistrationService(AppDbContext dbContext) : IEventRegistrationService
    {
        public async Task<EventRegistrationResult> RegisterAsync(Guid eventId, string userId, CancellationToken cancellationToken)
        {
            var strategy = dbContext.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                var now = DateTimeOffset.UtcNow;

                await using var transaction = await dbContext.Database.BeginTransactionAsync(
                    IsolationLevel.ReadCommitted,
                    cancellationToken);

                var existingRegistration = await dbContext.EventRegistrations
                    .SingleOrDefaultAsync(
                        registration => registration.EventId == eventId && registration.UserId == userId,
                        cancellationToken);

                if (existingRegistration is { Status: RegistrationStatus.Confirmed })
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return new EventRegistrationResult(
                        EventRegistrationResultCode.AlreadyRegistered,
                        "You are already registered for this event.");
                }

                var affectedRows = await dbContext.Events
                    .Where(e =>
                        e.Id == eventId &&
                        e.IsPublished &&
                        e.RegisteredCount < e.Capacity &&
                        (e.RegistrationOpensAtUtc == null || e.RegistrationOpensAtUtc <= now) &&
                        (e.RegistrationClosesAtUtc == null || e.RegistrationClosesAtUtc >= now))
                    .ExecuteUpdateAsync(
                        setters => setters.SetProperty(e => e.RegisteredCount, e => e.RegisteredCount + 1),
                        cancellationToken);

                if (affectedRows == 0)
                {
                    var unavailableResult = await GetUnavailableResultAsync(eventId, now, cancellationToken);
                    await transaction.RollbackAsync(cancellationToken);
                    return unavailableResult;
                }

                if (existingRegistration is { Status: RegistrationStatus.Cancelled })
                {
                    existingRegistration.Status = RegistrationStatus.Confirmed;
                    existingRegistration.RegisteredAtUtc = now;
                    existingRegistration.CancelledAtUtc = null;
                }
                else
                {
                    dbContext.EventRegistrations.Add(new EventRegistration
                    {
                        Id = Guid.NewGuid(),
                        EventId = eventId,
                        UserId = userId,
                        Status = RegistrationStatus.Confirmed,
                        RegisteredAtUtc = now
                    });
                }

                try
                {
                    await dbContext.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                }
                catch (DbUpdateException ex) when (ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return new EventRegistrationResult(
                        EventRegistrationResultCode.AlreadyRegistered,
                        "You are already registered for this event.");
                }

                return new EventRegistrationResult(
                    EventRegistrationResultCode.Registered,
                    "Registration confirmed.");
            });
        }

        private async Task<EventRegistrationResult> GetUnavailableResultAsync(
            Guid eventId,
            DateTimeOffset now,
            CancellationToken cancellationToken)
        {
            var eventSnapshot = await dbContext.Events
                .AsNoTracking()
                .Where(e => e.Id == eventId)
                .Select(e => new
                {
                    e.IsPublished,
                    e.Capacity,
                    e.RegisteredCount,
                    e.RegistrationOpensAtUtc,
                    e.RegistrationClosesAtUtc
                })
                .SingleOrDefaultAsync(cancellationToken);

            if (eventSnapshot is null || !eventSnapshot.IsPublished)
            {
                return new EventRegistrationResult(
                    EventRegistrationResultCode.EventNotFound,
                    "Event was not found.");
            }

            if (eventSnapshot.RegistrationOpensAtUtc > now || eventSnapshot.RegistrationClosesAtUtc < now)
            {
                return new EventRegistrationResult(
                    EventRegistrationResultCode.RegistrationClosed,
                    "Registration is closed for this event.");
            }

            if (eventSnapshot.RegisteredCount >= eventSnapshot.Capacity)
            {
                return new EventRegistrationResult(
                    EventRegistrationResultCode.EventFull,
                    "This event is full.");
            }

            return new EventRegistrationResult(
                EventRegistrationResultCode.RegistrationClosed,
                "Registration is not currently available for this event.");
        }
    }
}
