using System.Security.Claims;
using Icof.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Icof.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/events/{eventId:guid}/registrations")]
    public class EventRegistrationsController(IEventRegistrationService eventRegistrationService) : ControllerBase
    {
        [HttpPost("me")]
        public async Task<IActionResult> RegisterCurrentUser(Guid eventId, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            var result = await eventRegistrationService.RegisterAsync(eventId, userId, cancellationToken);

            return result.Code switch
            {
                EventRegistrationResultCode.Registered => Ok(result),
                EventRegistrationResultCode.AlreadyRegistered => Conflict(result),
                EventRegistrationResultCode.EventNotFound => NotFound(result),
                EventRegistrationResultCode.EventFull => Conflict(result),
                EventRegistrationResultCode.RegistrationClosed => BadRequest(result),
                _ => BadRequest(result)
            };
        }
    }
}
