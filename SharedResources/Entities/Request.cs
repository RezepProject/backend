namespace SharedResources.Entities;

public class Request
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Text { get; set; }
    public DateTime CreatedAt { get; } = DateTime.Now;
}