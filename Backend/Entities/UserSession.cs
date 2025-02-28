namespace backend.Entities;

public class UserSession
{
    public Guid SessionId { get; set; } = Guid.NewGuid();
    public string ChatGptThreadId { get; set; }
    public bool ProcessPersonalData { get; set; } = false;
    private string? _reservationUrl;

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