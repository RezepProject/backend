using System.ComponentModel.DataAnnotations;

namespace backend.Entities;

public class Role
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }

    public List<ConfigUser>? Users { get; set; }
}