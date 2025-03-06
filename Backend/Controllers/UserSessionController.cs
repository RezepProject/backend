using backend.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class UserSessionController(DataContext ctx) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<UserSession>> CreateUserSession([FromBody] CreateUserSession createUserSession)
    {
        var us = new UserSession()
        {
            FirstName = createUserSession.FirstName,
            LastName = createUserSession.LastName,
            ReservationStart = createUserSession.ReservationStart,
            ReservationEnd = createUserSession.ReservationEnd,
            ProcessPersonalData = createUserSession.ProcessPersonalData
        };
        ctx.UserSessions.Add(us);
        await ctx.SaveChangesAsync();
        return Created();
    }
    [HttpGet("{sessionId}")]
    public async Task<ActionResult<UserSession>> GetUserSession(string sessionId)
    {
        var us = await ctx.UserSessions.FindAsync(sessionId);
        if (us == null) return NotFound("UserSession not found!");
        return us;
    }
}