namespace backend.Entities;

public class Question
{
    public int Id { get; set; }
    public string Text { get; set; }
    
    public List<QuestionCategory> Categories { get; set; }

    public List<Answer>? Answers { get; set; }
}

public class CreateQuestion
{
    public string Text { get; set; }
    public List<CreateAnswer>? Answers { get; set; }
    
    public List<CreateQuestionCategory> Categories { get; set; }
}