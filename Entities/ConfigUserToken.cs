using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Entities;

public class ConfigUserToken
{
    public int Id { get; set; }
    public Guid Token { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public int RoleId { get; set; }
    public Role? Role { get; set; }
}

public class CreateUserToken
{
    public string Email { get; set; }
    public int RoleId { get; set; }
}