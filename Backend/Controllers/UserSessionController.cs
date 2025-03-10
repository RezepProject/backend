using backend.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        return Created(us.SessionId.ToString(), us);
    }

    [HttpGet("{sessionId:guid}")]
    public async Task<ActionResult<UserSession>> GetUserSession(Guid sessionId)
    {
        var us = await ctx.UserSessions.FirstOrDefaultAsync(u => u.SessionId == sessionId);
        if (us == null) return NotFound("UserSession not found!");
        return us;
    }

    [HttpPut]
    public async Task<ActionResult<UserSession>> UpdateUserSession([FromBody] UserSession userSession)
    {
        var us = await ctx.UserSessions.FindAsync(userSession.SessionId);
        if (us == null) return NotFound("UserSession not found!");

        us.ProcessPersonalData = userSession.ProcessPersonalData;
        us.FirstName = userSession.FirstName;
        us.LastName = userSession.LastName;
        us.ReservationStart = userSession.ReservationStart;
        us.ReservationEnd = userSession.ReservationEnd;

        await ctx.SaveChangesAsync();
        return userSession;
    }

    [HttpGet("reservation/{sessionId:guid}")]
    public async Task<ActionResult<ReservationDTO>> GetReservation(Guid sessionId)
    {
        var us = await ctx.UserSessions.FirstOrDefaultAsync(u => u.SessionId == sessionId);
        if (us == null) return NotFound("UserSession not found!");
        var res = await us.GetUserReservation();
        if(res == null) return NotFound("Reservation not found!");
        return new ReservationDTO(res);
    }

    [HttpPost("reservation/checkin/{sessionId:guid}")]
    public async Task<ActionResult<bool>> CheckIn(Guid sessionId)
    {
        var us = await ctx.UserSessions.FirstOrDefaultAsync(u => u.SessionId == sessionId);
        if (us == null) return NotFound("UserSession not found!");
        return await us.CheckIn();
    }

    [HttpPost("reservation/checkout/{sessionId:guid}")]
    public async Task<ActionResult<bool>> CheckOut(Guid sessionId)
    {
        var us = await ctx.UserSessions.FirstOrDefaultAsync(u => u.SessionId == sessionId);
        if (us == null) return NotFound("UserSession not found!");
        return await us.CheckOut();
    }
}