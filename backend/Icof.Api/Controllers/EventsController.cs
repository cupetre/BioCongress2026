using Icof.Api.Data;
using Icof.Api.DTOs;
using Icof.Api.Entities;
using Icof.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Icof.Api.Controllers;

// Backs Workshops, Lectures, Social Programs and Timetable — all four pages pull from this same
// Events table. Workshops/Lectures/Social Programs each filter by a single EventType;
// Timetable omits the filter and groups everything by day client-side.
[ApiController]
[Route("api/events")]
public class EventsController : ControllerBase
{
    private readonly AppDbContext _db;

    public EventsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<EventDto>>> GetEvents(
        [FromQuery] EventType? type,
        CancellationToken cancellationToken)
    {
        var query = _db.Events.Where(e => e.IsPublished);

        if (type is not null)
        {
            query = query.Where(e => e.Type == type.Value);
        }

        var events = await query
            .OrderBy(e => e.StartsAtUtc)
            .ThenBy(e => e.DisplayOrder)
            .ToListAsync(cancellationToken);

        return Ok(events.Select(ToDto).ToList());
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<EventDto>> Create(
        [FromBody] CreateEventRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return BadRequest("Title is required.");
        }

        if (request.Capacity < 0)
        {
            return BadRequest("Capacity cannot be negative.");
        }

        if (request.RegisteredCount < 0 || request.RegisteredCount > request.Capacity)
        {
            return BadRequest("RegisteredCount must be between 0 and Capacity.");
        }

        var displayOrder = request.DisplayOrder
            ?? (await _db.Events
                .Where(e => e.Type == request.Type)
                .Select(e => (int?)e.DisplayOrder)
                .MaxAsync(cancellationToken) ?? -1) + 1;

        var ev = new Event
        {
            Id = Guid.NewGuid(),
            Title = request.Title.Trim(),
            Slug = SlugHelper.Slugify(request.Title),
            Summary = request.Summary,
            Description = request.Description,
            Location = request.Location,
            Room = request.Room,
            Type = request.Type,
            Status = request.Status,
            StartsAtUtc = request.StartsAtUtc,
            EndsAtUtc = request.EndsAtUtc,
            RegistrationOpensAtUtc = request.RegistrationOpensAtUtc,
            RegistrationClosesAtUtc = request.RegistrationClosesAtUtc,
            Capacity = request.Capacity,
            RegisteredCount = request.RegisteredCount,
            IsRegistrationEnabled = request.IsRegistrationEnabled,
            EligibilityNotes = request.EligibilityNotes,
            RegistrationCtaLabel = request.RegistrationCtaLabel,
            IsPublished = true,
            DisplayOrder = displayOrder,
            CreatedAtUtc = DateTimeOffset.UtcNow
        };

        _db.Events.Add(ev);

        try
        {
            await _db.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            return Conflict($"An event with slug \"{ev.Slug}\" already exists — try a more specific title.");
        }

        return CreatedAtAction(nameof(GetEvents), new { type = ev.Type }, ToDto(ev));
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("{id:guid}")]
    public async Task<ActionResult<EventDto>> Update(
        Guid id,
        [FromBody] UpdateEventRequest request,
        CancellationToken cancellationToken)
    {
        var ev = await _db.Events.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        if (ev is null)
        {
            return NotFound();
        }

        if (request.Title is not null)
        {
            ev.Title = request.Title.Trim();
            ev.Slug = SlugHelper.Slugify(request.Title);
        }

        if (request.Summary is not null) ev.Summary = request.Summary;
        if (request.Description is not null) ev.Description = request.Description;
        if (request.Location is not null) ev.Location = request.Location;
        if (request.Room is not null) ev.Room = request.Room;
        if (request.Type is not null) ev.Type = request.Type.Value;
        if (request.Status is not null) ev.Status = request.Status.Value;
        if (request.StartsAtUtc is not null) ev.StartsAtUtc = request.StartsAtUtc.Value;
        if (request.EndsAtUtc is not null) ev.EndsAtUtc = request.EndsAtUtc;
        if (request.RegistrationOpensAtUtc is not null) ev.RegistrationOpensAtUtc = request.RegistrationOpensAtUtc;
        if (request.RegistrationClosesAtUtc is not null) ev.RegistrationClosesAtUtc = request.RegistrationClosesAtUtc;
        if (request.Capacity is not null) ev.Capacity = request.Capacity.Value;
        if (request.RegisteredCount is not null) ev.RegisteredCount = request.RegisteredCount.Value;
        if (request.IsRegistrationEnabled is not null) ev.IsRegistrationEnabled = request.IsRegistrationEnabled.Value;
        if (request.EligibilityNotes is not null) ev.EligibilityNotes = request.EligibilityNotes;
        if (request.RegistrationCtaLabel is not null) ev.RegistrationCtaLabel = request.RegistrationCtaLabel;
        if (request.DisplayOrder is not null) ev.DisplayOrder = request.DisplayOrder.Value;
        if (request.IsPublished is not null) ev.IsPublished = request.IsPublished.Value;

        if (ev.RegisteredCount > ev.Capacity)
        {
            return BadRequest("RegisteredCount cannot exceed Capacity.");
        }

        ev.UpdatedAtUtc = DateTimeOffset.UtcNow;

        try
        {
            await _db.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            return Conflict($"An event with slug \"{ev.Slug}\" already exists — try a more specific title.");
        }

        return Ok(ToDto(ev));
    }

    private static EventDto ToDto(Event e) => new()
    {
        Id = e.Id,
        Slug = e.Slug,
        Title = e.Title,
        Summary = e.Summary,
        Room = e.Room,
        Location = e.Location,
        Type = e.Type.ToString(),
        Status = e.Status.ToString(),
        StartsAtUtc = e.StartsAtUtc,
        EndsAtUtc = e.EndsAtUtc,
        IsRegistrationEnabled = e.IsRegistrationEnabled,
        Capacity = e.Capacity,
        RegisteredCount = e.RegisteredCount,
        DisplayOrder = e.DisplayOrder
    };
}
