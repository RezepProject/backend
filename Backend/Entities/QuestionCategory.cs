namespace backend.Entities;

public class QuestionCategory
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public List<Question> Questions { get; set; }
}

public class CreateQuestionCategory
{
    public string Name { get; set; }
}