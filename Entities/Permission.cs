using System.ComponentModel.DataAnnotations;

namespace backend.Entities;

public class Permission
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
}