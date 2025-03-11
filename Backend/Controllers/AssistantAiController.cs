using backend.Entities;
using backend.Util;
using Microsoft.EntityFrameworkCore;
using Task = backend.Entities.Task;

namespace backend.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class AssistantAiRouter(DataContext ctx) : ControllerBase
{
    private static MistralUtil _mistralUtil = new();
    public class UserRequest
    {
        public string Question { get; set; }
        public string? SessionId { get; set; }
        public string? Language { get; set; }
        public Guid? UserSession { get; set; }
    }

    public class UserResponse
    {
        public string Answer { get; set; }
        public string SessionId { get; set; }
        public string TimeNeeded { get; set; }
        public Guid? UserSessionId { get; set; }
        public bool Reservation { get; set; }
    }


    [HttpPost]
    public async Task<ActionResult<UserResponse>> GetAiResponse([FromBody] UserRequest userRequest)
    {
        DateTime start = DateTime.Now;
        string question = userRequest.Question;
        string? sessionId = userRequest.SessionId;
        string language = userRequest.Language ?? "en-US";

        var aiUtil = AiUtil.GetInstance();
        var us = await ctx.UserSessions.FirstOrDefaultAsync(us => us.SessionId == userRequest.UserSession);

        var (runId, threadId, userSession) = await aiUtil.AskQuestion(ctx, sessionId, question, language, userSession: us);

        await aiUtil.WaitForResult(threadId, runId);
        var response = await aiUtil.GetResultString(threadId, userSession);
        
        if (response.Contains("{Task:"))
        {
            var startIndex = response.IndexOf("{Task:", StringComparison.Ordinal) + 6;
            var endIndex = response.IndexOf("}", startIndex, StringComparison.Ordinal);
            var taskText = response.Substring(startIndex, endIndex - startIndex).Trim();

            response = response.Remove(response.IndexOf("{Task:", StringComparison.Ordinal), endIndex - response.IndexOf("{Task:", StringComparison.Ordinal) + 1).Trim();

            var newTask = new Task
            {
                Text = taskText,
                Done = false
            };

            ctx.Tasks.Add(newTask);
            await ctx.SaveChangesAsync();

            Console.WriteLine($"Task created: {newTask.Text}"); 
        }

        UserResponse userResponse = new UserResponse()
        {
            Answer = response,
            SessionId = threadId,
            TimeNeeded = (DateTime.Now - start).TotalSeconds.ToString(),
            UserSessionId = userSession.SessionId,
        };

        if (response.Contains("{Reservation"))
        {
            // {Reservation {firstName: <firstName>}, {lastName: <lastName>}, {checkinDate: <checkinDate>}, checkoutDate: <checkoutDate>}}}
            var firstName = response.Split("{firstName: ")[1].Split("},")[0];
            var lastName = response.Split("{lastName: ")[1].Split("},")[0];
            var checkinDate = response.Split("{checkinDate: ")[1].Split("},")[0];
            var checkoutDate = response.Split("{checkoutDate: ")[1].Split("}")[0];

            var session = await ctx.UserSessions.FirstOrDefaultAsync(u => u.SessionId.ToString() == sessionId);
            if (session == null)
            {
                session = new UserSession
                {
                    FirstName = firstName,
                    LastName = lastName,
                    ReservationStart = DateOnly.Parse(checkinDate),
                    ReservationEnd = DateOnly.Parse(checkoutDate),
                    ProcessPersonalData = true
                };
                ctx.UserSessions.Add(session);

                userResponse.UserSessionId = session.SessionId;
            }
            else
            {
                session.FirstName = firstName;
                session.LastName = lastName;
                session.ReservationStart = DateOnly.Parse(checkinDate);
                session.ReservationEnd = DateOnly.Parse(checkoutDate);
                session.ProcessPersonalData = true;
            }

            await ctx.SaveChangesAsync();

            userResponse.Reservation = true;
            userResponse.Answer = response.Split("}}}")[1];
        }

        return Ok(userResponse);
    }

    [HttpPost("mistral")]
    public async Task<ActionResult<UserResponse>> GetAiResponseMistral([FromBody] MistralUserQuestion question)
    {
        var (answer, thread) = await _mistralUtil.AskQuestion(ctx, question.ThreadId, question.Question);
        
        return Ok(new { Answer = answer, ThreadId = thread });
    }

    [HttpPost("process-personal-data/{id:guid}")]
    public async Task<ActionResult> SetPersonalData(Guid id, [FromBody] bool value)
    {
        var us = await ctx.UserSessions.FirstOrDefaultAsync(us => us.SessionId == id);
        if (us != null)
        {
            us.ProcessPersonalData = value;
            await ctx.SaveChangesAsync();
            return Ok();
        }
        return NotFound();
    }
}