namespace backend.Entities;

public class MistralThread
{
    public readonly Guid Id = Guid.NewGuid();
    public List<Message> Messages { get; } = new();
}

public class Message
{
    public string role { get; set; }
    public string content { get; set; }
}

public class MistralUserQuestion
{
    public string Question { get; set; }
    public string? ThreadId { get; set; }
}