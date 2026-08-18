using Icof.Api.Data;
using Icof.Api.DTOs;
using Icof.Api.Entities;
using Icof.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Icof.Api.Controllers;

// Public read-only endpoint backing the Members and Ambassadors pages. Both pages pull from the
// same PeopleGroup/TeamMember tables, filtered by PeopleGroupType — Members uses MemberGroup,
// Ambassadors uses AmbassadorGroup. Only published groups/members are returned.
[ApiController]
[Route("api/people-groups")]
public class PeopleGroupsController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IBlobStorageService _blobStorage;

    public PeopleGroupsController(AppDbContext db, IBlobStorageService blobStorage)
    {
        _db = db;
        _blobStorage = blobStorage;
    }

    [HttpGet]
    public async Task<ActionResult<List<PeopleGroupDto>>> GetGroups(
        [FromQuery] PeopleGroupType type,
        CancellationToken cancellationToken)
    {
        var groups = await _db.PeopleGroups
            .Where(g => g.Type == type && g.IsPublished)
            .Include(g => g.Members.Where(m => m.IsPublished))
            .OrderBy(g => g.DisplayOrder)
            .ToListAsync(cancellationToken);

        var result = groups.Select(g => new PeopleGroupDto
        {
            Id = g.Id,
            Slug = g.Slug,
            Name = g.Name,
            Description = g.Description,
            DisplayOrder = g.DisplayOrder,
            Members = g.Members
                .OrderBy(m => m.DisplayOrder)
                .Select(m => new TeamMemberDto
                {
                    Id = m.Id,
                    Slug = m.Slug,
                    FullName = m.FullName,
                    RoleTitle = m.RoleTitle,
                    Institution = m.Institution,
                    ShortBio = m.ShortBio,
                    Bio = m.Bio,
                    PhotoUrl = m.PhotoBlobName is not null ? _blobStorage.GetPublicUrl(m.PhotoBlobName) : null,
                    DisplayOrder = m.DisplayOrder
                })
                .ToList()
        }).ToList();

        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<PeopleGroupDto>> CreateGroup(
        [FromBody] CreatePeopleGroupRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest("Name is required.");
        }

        var displayOrder = request.DisplayOrder
            ?? (await _db.PeopleGroups
                .Where(g => g.Type == request.Type)
                .Select(g => (int?)g.DisplayOrder)
                .MaxAsync(cancellationToken) ?? -1) + 1;

        var group = new PeopleGroup
        {
            Id = Guid.NewGuid(),
            Type = request.Type,
            Name = request.Name.Trim(),
            Slug = SlugHelper.Slugify(request.Name),
            Description = request.Description,
            DisplayOrder = displayOrder,
            IsPublished = true,
            CreatedAtUtc = DateTimeOffset.UtcNow
        };

        _db.PeopleGroups.Add(group);

        try
        {
            await _db.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            return Conflict($"A group with slug \"{group.Slug}\" already exists — try a more specific name.");
        }

        var dto = new PeopleGroupDto
        {
            Id = group.Id,
            Slug = group.Slug,
            Name = group.Name,
            Description = group.Description,
            DisplayOrder = group.DisplayOrder,
            Members = new List<TeamMemberDto>()
        };

        return CreatedAtAction(nameof(GetGroups), new { type = group.Type }, dto);
    }
}
