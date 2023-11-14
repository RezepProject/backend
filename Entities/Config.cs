using System.ComponentModel.DataAnnotations;

namespace backend.Entities;

public class Config
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; }
    public string Value { get; set; }
}