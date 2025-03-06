using System.Net.Http.Headers;
using backend.Util;

namespace backend.Entities;

public class UserSession
{
    public Guid SessionId { get; set; } = Guid.NewGuid();
    public string? ChatGptThreadId { get; set; }
    public bool ProcessPersonalData { get; set; } = false;
    private string? _reservationId;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateOnly? ReservationStart { get; set; }
    public DateOnly? ReservationEnd { get; set; }

    public string? ReservationId
    {
        get => _reservationId;
        set
        {
            if (ProcessPersonalData)
            {
                _reservationId = value;
            }
        }
    }

    public Task<Reservation?> GetUserReservation()
    {
       return ApaleoUtil.GetInstance().GetReservation(ReservationId);
    }

    public Task<bool> CheckIn()
    {
        return ApaleoUtil.GetInstance().CheckIn(ReservationId);
    }

    public Task<bool> CheckOut()
    {
        return ApaleoUtil.GetInstance().CheckOut(ReservationId);
    }

    public async Task<bool> GetReservationId(string firstName, string lastName, string from, string to)
    {
        if (!ProcessPersonalData) return false;
        _reservationId = await ApaleoUtil.GetInstance().GetReservationId("BER", from, to, firstName, lastName);
        return _reservationId != null;
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