using System.Net.Http.Headers;
using System.Text;
using backend.Entities;
using backend.Util;
using Newtonsoft.Json;

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
    }

    public class UserResponse
    {
        public string Answer { get; set; }
        public string SessionId { get; set; }
        public string TimeNeeded { get; set; }
    }


    [HttpPost]
    public async Task<ActionResult<UserResponse>> GetAiResponse([FromBody] UserRequest userRequest)
    {
        DateTime start = DateTime.Now;
        string question = userRequest.Question;
        string? sessionId = userRequest.SessionId;
        string language = userRequest.Language ?? "en-US";

        var aiUtil = AiUtil.GetInstance();

        var (runId, threadId) = await aiUtil.AskQuestion(ctx, sessionId, question, language);

        await aiUtil.WaitForResult(threadId, runId);
        var response = await aiUtil.GetResultString(threadId);
        
        if (response.Contains("{Task:"))
        {
            var startIndex = response.IndexOf("{Task:", StringComparison.Ordinal) + 6;
            var endIndex = response.IndexOf("}", startIndex, StringComparison.Ordinal);
            var taskText = response.Substring(startIndex, endIndex - startIndex).Trim();

            response = response.Remove(response.IndexOf("{Task:", StringComparison.Ordinal), endIndex - response.IndexOf("{Task:", StringComparison.Ordinal) + 1).Trim();

            var newTask = new EntityTask
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
            TimeNeeded = (DateTime.Now - start).TotalSeconds.ToString()
        };

        return Ok(userResponse);
    }

    [HttpPost("mistral")]
    public async Task<ActionResult<UserResponse>> GetAiResponseMistral([FromBody] MistralUserQuestion question)
    {
        var (answer, thread) = await _mistralUtil.AskQuestion(ctx, question.ThreadId, question.Question);
        
        return Ok(new { Answer = answer, ThreadId = thread });
    }
}