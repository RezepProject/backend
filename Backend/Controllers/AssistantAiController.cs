using System.Net.Http.Headers;
using System.Text;
using backend.Util;
using Newtonsoft.Json;

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

        var (runId, threadId) = await aiUtil.AskQuestion(sessionId, question, language);

        bool isCompleted = false;
        bool firstRun = true;
        while (!isCompleted)
        {
            if (!firstRun)
            {
                await Task.Delay(500);
            }

            firstRun = false;
            isCompleted = await aiUtil.CheckStatus(threadId, runId);
        }

        UserResponse userResponse = new UserResponse()
        {
            Answer = await aiUtil.GetResultString(threadId),
            SessionId = threadId,
            TimeNeeded = (DateTime.Now - start).TotalSeconds.ToString()
        };

        return Ok(userResponse);
    }

    [HttpPost("mistral")]
    public async Task<ActionResult<string>> GetAiResponseMistral([FromBody] string question)
    {
        string key = Program.config["MistralAi:Key"];
        using var httpClient = new HttpClient();

        var requestData = new
        {
            agent_id = "ag:0f95780e:20240929:untitled-agent:fd65062b",
            messages = new[]
            {
                new
                {
                    role = "user",
                    content = question
                }
            }
        };

        var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", key);

        var response = await httpClient.PostAsync("https://api.mistral.ai/v1/agents/completions", content);
        var responseContent = await response.Content.ReadAsStringAsync();

        return Ok(responseContent);
    }
}