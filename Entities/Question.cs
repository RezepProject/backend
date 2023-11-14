using System.ComponentModel.DataAnnotations;

namespace backend.Entities;

public class Question
{
    [Key]
    public int Id { get; set; }
    public string Text { get; set; }

    public List<Answer>? Answers { get; set; }
}