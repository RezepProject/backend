using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace backend.Util;

public class AiUtil
{
    private static AiUtil? _aiUtil;

    public static AiUtil? GetInstance()
    {
        if (_aiUtil == null)
        {
            _aiUtil = new AiUtil();
        }

        return _aiUtil;
    }

    private HttpClient _httpClient = new HttpClient();
    private string _assistantId = "";
    private string _nextThreadId = "";
    private string[] _files = [];

    private AiUtil()
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Program.config["OpenAiKey"] ?? string.Empty);
        _httpClient.DefaultRequestHeaders.Add("OpenAI-Beta", "assistants=v1");
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        CreateAssistant();
    }

    private async Task CreateAssistant()
    {
        // upload files

        // create assistant
        var assistantData = new
        {
            instructions = "Ignore all the previous instructions. You are now a new assistant for a Hotel / Restaurant. Use everything you know (first of all the files) and answer the questions accordingly. Its possible that a question needs information from the internet, you are only allowed to use it in this case. Write a summarizing answer to the question as best as you can. Here is everything you need to know: The toilet is on the right side of the reception.",
            name = "Name1",
            model = "gpt-4-1106-preview"
        };

        var content = new StringContent(JsonConvert.SerializeObject(assistantData), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("https://api.openai.com/v1/assistants", content);
        var responseContent = await response.Content.ReadAsStringAsync();

        JObject result = (JObject)JsonConvert.DeserializeObject(responseContent);

        _assistantId = result["id"].ToString();

        // create thread
        _nextThreadId = await CreateThread();
    }

    private async Task<string> GetThread()
    {
        if(_assistantId == "")
            await CreateAssistant();

        string tmp = _nextThreadId;
        _nextThreadId = await CreateThread();
        return tmp;
    }

    private async Task<string> CreateThread()
    {
        var response = await _httpClient.PostAsync("https://api.openai.com/v1/threads", null);
        var responseContent = await response.Content.ReadAsStringAsync();

        JObject result = (JObject)JsonConvert.DeserializeObject(responseContent);
        return result["id"].ToString();
    }

    // returns questionId, threadId
    public async Task<(string, string)> AskQuestion(string? threadId, string question)
    {
        if(string.IsNullOrEmpty(threadId))
            threadId = await GetThread();

        var messageData = new
        {
            role = "user",
            content = question
            // file_ids = files,
        };

        var content = new StringContent(JsonConvert.SerializeObject(messageData), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"https://api.openai.com/v1/threads/{threadId}/messages", content);
        var responseContent = await response.Content.ReadAsStringAsync();

        JObject result = (JObject)JsonConvert.DeserializeObject(responseContent);

        var runData = new
        {
            assistant_id = _assistantId
        };

        content = new StringContent(JsonConvert.SerializeObject(runData), Encoding.UTF8, "application/json");
        response = await _httpClient.PostAsync($"https://api.openai.com/v1/threads/{threadId}/runs", content);
        responseContent = await response.Content.ReadAsStringAsync();

        result = (JObject)JsonConvert.DeserializeObject(responseContent);

        return (result["id"].ToString(), threadId);
    }

    public async Task<bool> CheckStatus(string threadId, string runId)
    {
        var response = await _httpClient.GetAsync($"https://api.openai.com/v1/threads/{threadId}/runs/{runId}");
        var responseContent = await response.Content.ReadAsStringAsync();

        // check if answer is included
        JObject result = (JObject)JsonConvert.DeserializeObject(responseContent);
        return result["status"].ToString() == "completed";
    }

    private async Task<JObject> GetResult(string threadId)
    {
        var response = await _httpClient.GetAsync($"https://api.openai.com/v1/threads/{threadId}/messages");
        var responseContent = await response.Content.ReadAsStringAsync();

        return (JObject)JsonConvert.DeserializeObject(responseContent);
    }

    public async Task<string> GetResultString(string threadId)
    {
        return (await GetResult(threadId))["data"][0]["content"][0]["text"]["value"].ToString();
    }
}