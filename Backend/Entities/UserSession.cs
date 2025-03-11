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
        get
        {
            if (!ProcessPersonalData) return null;
            if (_reservationId == null)
            {
                _reservationId = GetReservationId().Result;
            }

            return _reservationId;
        }
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

    private async Task<string?> GetReservationId()
    {
        if (!ProcessPersonalData) return null;

        return await ApaleoUtil.GetInstance()
            .GetReservationId("BER", ReservationStart, ReservationEnd, FirstName, LastName);
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