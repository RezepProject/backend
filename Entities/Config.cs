using System.ComponentModel.DataAnnotations;

namespace backend.Entities;

public class Config
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; }
    public string Value { get; set; }
}

public class CreateConfig
{
    [Required]
    public string Title { get; set; }
    [Required]
    public string Value { get; set; }
}