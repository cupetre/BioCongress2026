using Icof.Api.Data;
using Icof.Api.DTOs;
using Icof.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Icof.Api.Controllers;

// Public read-only endpoint backing Workshops, Lectures, Social Programs and Timetable — all
// four pages pull from this same Events table. Workshops/Lectures/Social Programs each filter
// by a single EventType; Timetable omits the filter and groups everything by day client-side.
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

        var result = events.Select(e => new EventDto
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
        }).ToList();

        return Ok(result);
    }
}
