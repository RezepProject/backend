using backend.Util;

namespace backend.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class AssistantAiRouter(DataContext ctx) : ControllerBase
{
    public class UserRequest
    {
        public string Question { get; set; }
        public string? SessionId { get; set; }
    }

    public class UserResponse
    {
        public string Answer { get; set; }
        public string SessionId { get; set; }
    }

    [HttpPost]
    public async Task<ActionResult<UserResponse>> GetAiResponse([FromBody] UserRequest userRequest)
    {
        string? question = userRequest.Question;
        string? sessionId = userRequest.SessionId;

        var aiUtil = AiUtil.GetInstance();

        var (runId, threadId) = await aiUtil.AskQuestion(sessionId, question);

        bool isCompleted = false;
        while (!isCompleted)
        {
            isCompleted = await aiUtil.CheckStatus(threadId, runId);
            await Task.Delay(1000);
        }

        UserResponse userResponse = new UserResponse()
        {
            Answer = await aiUtil.GetResultString(threadId),
            SessionId = threadId
        };

        return Ok(userResponse);
    }
}