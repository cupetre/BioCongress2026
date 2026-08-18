using Icof.Api.Data;
using Icof.Api.DTOs;
using Icof.Api.Entities;
using Icof.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Icof.Api.Controllers;

[ApiController]
[Route("api/team-members")]
public class TeamMembersController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IBlobStorageService _blobStorage;

    public TeamMembersController(AppDbContext db, IBlobStorageService blobStorage)
    {
        _db = db;
        _blobStorage = blobStorage;
    }

    // TODO: restrict to an Admin role once roles are seeded/assigned — [Authorize] just
    // requires *some* logged-in user for now, same interim approach as ImageController.
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<TeamMemberDto>> Create(
        [FromBody] CreateTeamMemberRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.FullName))
        {
            return BadRequest("FullName is required.");
        }

        if (request.PeopleGroupId is null && string.IsNullOrWhiteSpace(request.PeopleGroupSlug))
        {
            return BadRequest("Either peopleGroupId or peopleGroupSlug is required.");
        }

        var group = request.PeopleGroupId is not null
            ? await _db.PeopleGroups.FirstOrDefaultAsync(g => g.Id == request.PeopleGroupId, cancellationToken)
            : await _db.PeopleGroups.FirstOrDefaultAsync(g => g.Slug == request.PeopleGroupSlug, cancellationToken);

        if (group is null)
        {
            return BadRequest("No matching people group found for the given peopleGroupId/peopleGroupSlug.");
        }

        var displayOrder = request.DisplayOrder
            ?? (await _db.TeamMembers
                .Where(m => m.PeopleGroupId == group.Id)
                .Select(m => (int?)m.DisplayOrder)
                .MaxAsync(cancellationToken) ?? -1) + 1;

        var member = new TeamMember
        {
            Id = Guid.NewGuid(),
            PeopleGroupId = group.Id,
            FullName = request.FullName.Trim(),
            Slug = SlugHelper.Slugify(request.FullName),
            RoleTitle = request.RoleTitle,
            Institution = request.Institution,
            ShortBio = request.ShortBio,
            Bio = request.Bio,
            PhotoBlobName = request.PhotoBlobName,
            DisplayOrder = displayOrder,
            IsPublished = true,
            CreatedAtUtc = DateTimeOffset.UtcNow
        };

        _db.TeamMembers.Add(member);

        try
        {
            await _db.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            return Conflict($"A member with slug \"{member.Slug}\" already exists — try a more specific full name.");
        }

        var dto = new TeamMemberDto
        {
            Id = member.Id,
            Slug = member.Slug,
            FullName = member.FullName,
            RoleTitle = member.RoleTitle,
            Institution = member.Institution,
            ShortBio = member.ShortBio,
            Bio = member.Bio,
            PhotoUrl = member.PhotoBlobName is not null ? _blobStorage.GetPublicUrl(member.PhotoBlobName) : null,
            DisplayOrder = member.DisplayOrder
        };

        return CreatedAtAction(nameof(Create), new { id = member.Id }, dto);
    }

    // TODO: restrict to an Admin role once roles are seeded/assigned.
    [Authorize]
    [HttpPatch("{id:guid}")]
    public async Task<ActionResult<TeamMemberDto>> Update(
        Guid id,
        [FromBody] UpdateTeamMemberRequest request,
        CancellationToken cancellationToken)
    {
        var member = await _db.TeamMembers.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        if (member is null)
        {
            return NotFound();
        }

        if (request.FullName is not null)
        {
            member.FullName = request.FullName.Trim();
            member.Slug = SlugHelper.Slugify(request.FullName);
        }

        if (request.RoleTitle is not null) member.RoleTitle = request.RoleTitle;
        if (request.Institution is not null) member.Institution = request.Institution;
        if (request.ShortBio is not null) member.ShortBio = request.ShortBio;
        if (request.Bio is not null) member.Bio = request.Bio;
        if (request.PhotoBlobName is not null) member.PhotoBlobName = request.PhotoBlobName;
        if (request.DisplayOrder is not null) member.DisplayOrder = request.DisplayOrder.Value;
        if (request.IsPublished is not null) member.IsPublished = request.IsPublished.Value;

        member.UpdatedAtUtc = DateTimeOffset.UtcNow;

        try
        {
            await _db.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            return Conflict($"A member with slug \"{member.Slug}\" already exists — try a more specific full name.");
        }

        var dto = new TeamMemberDto
        {
            Id = member.Id,
            Slug = member.Slug,
            FullName = member.FullName,
            RoleTitle = member.RoleTitle,
            Institution = member.Institution,
            ShortBio = member.ShortBio,
            Bio = member.Bio,
            PhotoUrl = member.PhotoBlobName is not null ? _blobStorage.GetPublicUrl(member.PhotoBlobName) : null,
            DisplayOrder = member.DisplayOrder
        };

        return Ok(dto);
    }
}
