using System.Net.Http.Headers;
using System.Text;
using backend.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace backend.Util;

public class MistralUtil
{
    private static readonly List<MistralThread> Threads = new();
    private static readonly string Key = Program.config["MistralAi:Key"];
    private static readonly HttpClient HttpClient = new();

    public async Task<(string, string)> AskQuestion(string? threadId, string question)
    {
        var thread = string.IsNullOrEmpty(threadId) || Threads.All(t => t.Id.ToString() != threadId)
            ? CreateNewThread(out threadId)
            : Threads.First(t => t.Id.ToString() == threadId);

        thread.Messages.Add(new Message { Role = "user", Content = question });

        var requestData = new
        {
            agent_id = "ag:0f95780e:20240929:untitled-agent:fd65062b",
            messages = thread.Messages.Select(m => new { role = m.Role, content = m.Content }).ToArray()
        };

        var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
        HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Key);

        var response = await HttpClient.PostAsync("https://api.mistral.ai/v1/agents/completions", content);
        var responseContent = await response.Content.ReadAsStringAsync();

        var jsonResponse = JObject.Parse(responseContent);
        var message = jsonResponse["choices"]?[0]?["message"];
        if (message != null)
        {
            var role = message["role"]?.ToString();
            var contentMessage = message["content"]?.ToString();
            if (role != null && contentMessage != null)
            {
                thread.Messages.Add(new Message { Role = role, Content = contentMessage });
            }
        }

        return (responseContent, threadId);
    }

    private static MistralThread CreateNewThread(out string threadId)
    {
        var newThread = new MistralThread();
        Threads.Add(newThread);
        threadId = newThread.Id.ToString();
        return newThread;
    }
}