using System.ComponentModel.DataAnnotations;

namespace backend.Entities;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; }

    public List<ConfigUser>? Users { get; set; }
}

public class CreateRole
{
    public string Name { get; set; }
}