namespace backend.Entities;

public class UserSession
{
    public Guid SessionId { get; set; } = Guid.NewGuid();
    public string? ChatGptThreadId { get; set; }
    public bool ProcessPersonalData { get; set; } = false;
    private string? _reservationUrl;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateOnly? ReservationStart { get; set; }
    public DateOnly? ReservationEnd { get; set; }

    public string? ReservationUrl
    {
        get => _reservationUrl;
        set
        {
            if (ProcessPersonalData)
            {
                _reservationUrl = value;
            }
        }
    }

    public async Task<Reservation> GetUserReservation()
    {
        throw new NotImplementedException();
    }
}

public class CreateUserSession
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateOnly? ReservationStart { get; set; }
    public DateOnly? ReservationEnd { get; set; }
    public bool ProcessPersonalData { get; set; } = false;
}