namespace backend.Entities;

public class RefreshToken
{
    private DateTime _created = DateTime.Now;
    private DateTime _expires;
    public int Id { get; set; }
    public string Token { get; set; } = string.Empty;

    public DateTime Created
    {
        get => _created;
        set => _created = value.ToUniversalTime();
    }

    public DateTime Expires
    {
        get => _expires;
        set => _expires = value.ToUniversalTime();
    }
}