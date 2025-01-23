namespace backend.Entities;

public class Tasks
{
    public int Id { get; set; }
    public string Text { get; set; }
    public bool Done { get; set; }
}

public class CreateTask
{
    public string Text { get; set; }
    public bool Done { get; set; }
}

public class UpdateTask
{
    public string Text { get; set; }
    public bool Done { get; set; }
}
