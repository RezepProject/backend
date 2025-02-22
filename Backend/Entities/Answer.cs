namespace backend.Entities;

public class Answer : IEntity
{
    public int Id { get; set; }
    public string Text { get; set; }
    public string User { get; set; }
}

public class CreateAnswer : IEntity
{
    public string Text { get; set; }
    public string User { get; set; }
}

public class UpdateAnswer : IEntity
{
    public string Text { get; set; }
    public string User { get; set; }
}