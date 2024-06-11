using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace backend.Util;

internal class ThreadTime
{
    public string ThreadId { get; set; }
    public DateTime Time { get; set; }
}

public class AiUtil
{
    private static AiUtil? _aiUtil;
    private string _assistantId = "";

    private readonly HttpClient _httpClient = new();
    private string _nextThreadId = "";
    private readonly List<ThreadTime> _threads = new();

    private AiUtil()
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", Program.config["OpenAi:Key"] ?? string.Empty);
        _httpClient.DefaultRequestHeaders.Add("OpenAI-Beta", "assistants=v2");
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        CreateAssistant().Wait();
    }

    public static AiUtil GetInstance()
    {
        return _aiUtil ??= new AiUtil();
    }

    private async Task CreateAssistant()
    {
        // create assistant
        var assistantData = new
        {
            instructions = "Ignore all the previous instructions. " +
                           "You are now a new assistant for a Hotel / Restaurant. " +
                           "Use everything you know (first of all the files) and answer the questions accordingly. " +
                           "Its possible that a question needs information from the internet, " +
                           "you are only allowed to use it in this case. Write a summarizing answer to the question as " +
                           "best as you can. Here is everything you need to know: The toilet is on the right side " +
                           "of the reception. Always write short answers up to 50 words! " +
                           "You dont have to rewrite every answer, just answer the question faster instead.",
            name = "Rezep",
            model = "gpt-4-1106-preview"
        };

        var content = new StringContent(JsonConvert.SerializeObject(assistantData), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("https://api.openai.com/v1/assistants", content);
        var responseContent = await response.Content.ReadAsStringAsync();

        var result = (JObject)JsonConvert.DeserializeObject(responseContent)!;

        _assistantId = result["id"]!.ToString();

        // create thread
        _nextThreadId = await CreateThread();
    }

    private async Task<string> GetThread()
    {
        if (_assistantId == "")
            await CreateAssistant();

        var tmp = _nextThreadId;
        _nextThreadId = await CreateThread();
        return tmp;
    }

    private async Task UpdateThreads()
    {
        for (var i = 0; i < _threads.Count; i++)
            if ((DateTime.Now - _threads[i].Time).TotalMinutes > 1)
            {
                await DeleteThread(_threads[i].ThreadId);
                _threads.RemoveAt(i);
                i--;
            }
    }

    private async Task DeleteThread(string threadId)
    {
        await _httpClient.DeleteAsync($"https://api.openai.com/v1/threads/{threadId}");
    }

    private async Task<string> CreateThread()
    {
        var response = await _httpClient.PostAsync("https://api.openai.com/v1/threads", null);
        var responseContent = await response.Content.ReadAsStringAsync();

        var result = (JObject)JsonConvert.DeserializeObject(responseContent)!;
        return result["id"]!.ToString();
    }

    // returns questionId, threadId
    public async Task<(string, string)> AskQuestion(string? threadId, string question)
    {
        await UpdateThreads();
        if (string.IsNullOrEmpty(threadId) || _threads.Where(t => t.ThreadId == threadId) != null)
        {
            threadId = await GetThread();
            _threads.Add(new ThreadTime { ThreadId = threadId, Time = DateTime.Now });
        }
        else
        {
            _threads.FirstOrDefault(t => t.ThreadId == threadId)!.Time = DateTime.Now;
        }

        var messageData = new
        {
            role = "user",
            content = question
            // file_ids = files,
        };

        var content = new StringContent(JsonConvert.SerializeObject(messageData), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"https://api.openai.com/v1/threads/{threadId}/messages", content);
        var responseContent = await response.Content.ReadAsStringAsync();

        var result = (JObject)JsonConvert.DeserializeObject(responseContent)!;

        var runData = new
        {
            assistant_id = _assistantId
        };

        content = new StringContent(JsonConvert.SerializeObject(runData), Encoding.UTF8, "application/json");
        response = await _httpClient.PostAsync($"https://api.openai.com/v1/threads/{threadId}/runs", content);
        responseContent = await response.Content.ReadAsStringAsync();

        result = (JObject)JsonConvert.DeserializeObject(responseContent)!;

        return (result["id"]!.ToString(), threadId);
    }

    public async Task<bool> CheckStatus(string threadId, string runId)
    {
        var response = await _httpClient.GetAsync($"https://api.openai.com/v1/threads/{threadId}/runs/{runId}");
        var responseContent = await response.Content.ReadAsStringAsync();

        // check if answer is included
        var result = (JObject)JsonConvert.DeserializeObject(responseContent)!;
        return result["status"]!.ToString() == "completed";
    }

    private async Task<JObject> GetResult(string threadId)
    {
        var response = await _httpClient.GetAsync($"https://api.openai.com/v1/threads/{threadId}/messages");
        var responseContent = await response.Content.ReadAsStringAsync();

        return (JObject)JsonConvert.DeserializeObject(responseContent)!;
    }

    public async Task<string> GetResultString(string threadId)
    {
        return (await GetResult(threadId))["data"][0]["content"][0]["text"]["value"].ToString();
    }
}