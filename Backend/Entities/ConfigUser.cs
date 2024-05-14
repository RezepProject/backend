namespace backend.Entities;

public class ConfigUser
{
    private DateTime _tokenCreated;
    private DateTime _tokenExpires;
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public int RoleId { get; set; }
    public Role? Role { get; set; }
    public string RefreshToken { get; set; } = string.Empty;

    public DateTime TokenCreated
    {
        get => _tokenCreated;
        set => _tokenCreated = value.ToUniversalTime();
    }

    public DateTime TokenExpires
    {
        get => _tokenExpires;
        set => _tokenExpires = value.ToUniversalTime();
    }
}

public class CreateConfigUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    public Guid Token { get; set; }
}

public class ChangeConfigUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public int RoleId { get; set; }
}

public class ReturnConfigUser
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public int RoleId { get; set; }
}