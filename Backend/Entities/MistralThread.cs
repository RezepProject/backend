namespace backend.Entities;

public class MistralThread
{
    public readonly Guid Id = Guid.NewGuid();
    public List<Message> Messages { get; } = new();
}

public class Message
{
    public string Role { get; set; }
    public string Content { get; set; }
}

public class MistralUserQuestion
{
    public string Question { get; set; }
    public string? ThreadId { get; set; }
}