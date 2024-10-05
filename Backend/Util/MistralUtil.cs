using System.Net.Http.Headers;
using System.Text;
using backend.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace backend.Util;

public class MistralUtil
{
    private static readonly List<MistralThread> Threads = new();
    private static readonly string Key = Program.config["MistralAi:Key"];
    private static readonly HttpClient HttpClient = new();
    private List<QuestionCategory> _categories = new();

    public async Task<(string, string)> AskQuestion(DataContext ctx, string? threadId, string question)
    {
        var thread = string.IsNullOrEmpty(threadId) || Threads.All(t => t.Id.ToString() != threadId)
            ? CreateNewThread(out threadId)
            : Threads.First(t => t.Id.ToString() == threadId);

        thread.Messages.Add(new Message { Role = "user", Content = question });

        var classifyQuestions = await GetClassifyQuestions(ctx, question);

        List<Message> includedMessages = new();

        if (classifyQuestions.Count > 0)
        {
            var previousQnA = classifyQuestions
                .Select(q => $"{q.Text};{q.Answers?.FirstOrDefault()?.Text}")
                .Where(qna => !string.IsNullOrEmpty(qna))
                .Aggregate((a, b) => $"{a}\n{b}");

            includedMessages.Add(new Message
            {
                Role = "system",
                Content = $"Use the following information: {previousQnA}"
            });
        }

        includedMessages.AddRange(thread.Messages.Select(m => new Message { Role = m.Role, Content = m.Content })
            .ToList());

        var requestData = new
        {
            agent_id = "ag:0f95780e:20240929:untitled-agent:fd65062b",
            messages = includedMessages
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
                return(contentMessage, threadId);
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

    private async Task<List<Question>> GetClassifyQuestions(DataContext ctx, string question)
    {
        var tmpCategories = await ctx.QuestionCategories.ToListAsync();
        if (tmpCategories.Count != _categories.Count)
        {
            _categories = tmpCategories;
        }

        if (_categories.Count == 0)
        {
            var tmpQuestions = await ctx.Questions.ToListAsync();
            if (tmpQuestions.Count > 0)
            {
                List<QuestionCategory> categories = (await ClassifyQuestion(question));
                return tmpQuestions.Where(q => categories.Any(c => q.Categories.Any(c2 => c2.Name == c.Name))).ToList();
            }
        }

        return new List<Question>();
    }

    private async Task<List<QuestionCategory>> ClassifyQuestion(string question)
    {
        var requestData = new
        {
            agent_id = "ag:0f95780e:20241005:herbert-classifier:610dc95e",
            messages = new[]
            {
                new
                {
                    role = "system",
                    content = $"Your categories are: {_categories.Select(c => c.Name).Aggregate((a, b) => a + ";" + b)}"
                },
                new
                {
                    role = "user",
                    content = question
                }
            }
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
            var categories = contentMessage?.Split(";");
            if (role != null && contentMessage != null)
            {
                return _categories
                    .Where(c => categories
                        .Any(c2 => c.Name == c2))
                    .ToList();
            }
        }

        throw new Exception("Could not classify question");
    }
}