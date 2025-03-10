using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using backend.Entities;

namespace backend.Util;

public class ApaleoUtil
{
    private static ApaleoUtil? _instance;
    private static readonly object Lock = new();

    private string _token = "";

    private string Token
    {
        get
        {
            if (_token == "") _token = Authenticate().Result;
            return _token;
        }
    }

    private ApaleoUtil() { }

    public static ApaleoUtil GetInstance()
    {
        lock (Lock)
        {
            return _instance ??= new ApaleoUtil();
        }
    }

    private async Task<string> Authenticate()
    {
        var client = new HttpClient();
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "client_credentials")
        });

        var auth = Convert.ToBase64String("WQRH-SP-BOOKINGHERBERT:ZongcbgRr2HuYG8w2u7pn8ViuPaRdn"u8.ToArray());
        client.DefaultRequestHeaders.Add("Authorization", "Basic " + auth);

        var res = await client.PostAsync("https://identity.apaleo.com/connect/token", content);

        var json = await res.Content.ReadAsStringAsync();
        var token = JsonSerializer.Deserialize<ApaleoToken>(json);

        return token == null ? "" : token.Token;
    }

    public async Task<Reservation?> GetReservation(string? reservationId)
    {
        if(reservationId == null) return null;

        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

        var res = await client.GetAsync($"https://api.apaleo.com/booking/v1/reservations/{reservationId}?expand=booker");

        if (res.StatusCode == HttpStatusCode.Unauthorized)
        {
            _token = await Authenticate();
            return await GetReservation(reservationId);
        }

        var json = await res.Content.ReadAsStringAsync();
        return Reservation.FromJson(json);
    }

    public async Task<bool> CheckIn(string? reservationId)
    {
        if(await IsCheckedIn(reservationId)) return false;

        var client = new HttpClient();
        var content = new StringContent("", Encoding.UTF8, "application/json");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        var res = await client.PutAsync($"https://api.apaleo.com/booking/v1/reservation-actions/{reservationId}/checkin", content);

        if (res.StatusCode == HttpStatusCode.Unauthorized)
        {
            _token = await Authenticate();
            return await CheckIn(reservationId);
        }

        return res.IsSuccessStatusCode;
    }

    public async Task<bool> CheckOut(string? reservationId)
    {
        if (!await IsCheckedIn(reservationId)) return false;

        var client = new HttpClient();
        var content = new StringContent("", Encoding.UTF8, "application/json");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        var res = await client.PutAsync($"https://api.apaleo.com/booking/v1/reservation-actions/{reservationId}/checkout", content);

        if (res.StatusCode == HttpStatusCode.Unauthorized)
        {
            _token = await Authenticate();
            return await CheckOut(reservationId);
        }

        return res.IsSuccessStatusCode;
    }

    private async Task<bool> IsCheckedIn(string? reservationId)
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        var res = await client.GetAsync($"https://api.apaleo.com/booking/v1/reservations/{reservationId}");

        if (res.StatusCode == HttpStatusCode.Unauthorized)
        {
            _token = await Authenticate();
            return await IsCheckedIn(reservationId);
        }

        var json = await res.Content.ReadAsStringAsync();
        var reservation = Reservation.FromJson(json);

        return reservation?.Status == "InHouse";
    }

    public async Task<string?> GetReservationId(string? property, DateOnly? from, DateOnly? to, string? firstName,
        string? lastName)
    {
        if (property == null || from == null || to == null || firstName == null || lastName == null) return null;

        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        var res = await client.GetAsync($"https://api.apaleo.com/booking/v1/reservations?" +
                                        $"propertyIds={property}&dateFilter=DepartureAndCheckOut&from={ConvertDate((DateOnly)from)}%3A00%3A00%2B01%3A00" +
                                        $"&to={ConvertDate((DateOnly)to)}%3A00%3A00%2B01%3A00&pageNumber=1?expand=booker");


        if (res.StatusCode == HttpStatusCode.Unauthorized)
        {
            _token = await Authenticate();
            return await GetReservationId(property, from, to, firstName, lastName);
        }

        var json = await res.Content.ReadAsStringAsync();
        var reservationRes = JsonSerializer.Deserialize<ReservationResponse>(json);

        if (reservationRes != null && reservationRes.Count != 0)
            return reservationRes.Reservations.FirstOrDefault(r => r.PrimaryGuest.FirstName == firstName && r.PrimaryGuest.LastName == lastName)?.Id;

        return null;
    }

    private string ConvertDate(DateOnly date)
    {
        DateTime dateTime = date.ToDateTime(new TimeOnly(15, 0)); // Set time to 15:00
        DateTimeOffset dateTimeOffset = new DateTimeOffset(dateTime, TimeZoneInfo.Local.GetUtcOffset(dateTime));

        return dateTimeOffset.ToString("yyyy-MM-ddTHH");
    }
}