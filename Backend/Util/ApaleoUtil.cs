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

        var response = await client.PostAsync("https://identity.apaleo.com/connect/token", content);
        var json = await response.Content.ReadAsStringAsync();
        var token = JsonSerializer.Deserialize<IApaleoToken>(json);

        return token == null ? "" : token.Token;
    }

    public async Task<Reservation?> GetReservation(string? reservationId)
    {
        if(reservationId == null) return null;

        var client = new HttpClient();
        var response = await client.GetAsync($"https://api.apaleo.com/booking/v1/reservations/{reservationId}&expand=booker");

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        var json = await response.Content.ReadAsStringAsync();
        return Reservation.FromJson(json);
    }

    public async Task<bool> CheckIn(string reservationId)
    {
        // check if valid reservation date name etc.
        var client = new HttpClient();
        var content = new StringContent("", Encoding.UTF8, "application/json");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        var res = await client.PutAsync($"https://api.apaleo.com/booking/v1/reservation-actions/{reservationId}/checkin", content);

        return res.IsSuccessStatusCode;
    }

    public async Task<bool> CheckOut(string reservationId)
    {
        // check if valid reservation date name etc.
        var client = new HttpClient();
        var content = new StringContent("", Encoding.UTF8, "application/json");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        var res = await client.PutAsync($"https://api.apaleo.com/booking/v1/reservation-actions/{reservationId}/checkout", content);

        return res.IsSuccessStatusCode;
    }

    private async Task<bool> IsCheckedIn(string reservationId)
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        var res = await client.GetAsync($"https://api.apaleo.com/booking/v1/reservations/{reservationId}");
        var json = await res.Content.ReadAsStringAsync();
        var reservation = Reservation.FromJson(json);

        return reservation?.Status == "InHouse";
    }

    public async Task<string?> GetReservationId(string property, string from, string to, string firstName,
        string lastName)
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        var res = await client.GetAsync($"https://api.apaleo.com/booking/v1/reservations?" +
                                        $"propertyIds={property}&dateFilter=DepartureAndCheckOut&from={ConvertDate(from)}" +
                                        $"&to={ConvertDate(to)}&pageNumber=1&expand=booker");

        var json = await res.Content.ReadAsStringAsync();
        var reservations = JsonSerializer.Deserialize<List<Reservation>>(json);

        if (reservations != null && reservations.Count != 0)
            return reservations.FirstOrDefault(r => r.Booker.FirstName == firstName && r.Booker.LastName == lastName)?.Id;

        return null;
    }

    private string ConvertDate(string date)
    {
        DateTime now = DateTime.UtcNow.AddHours(1); // Austria is UTC+1 in standard time
        // Format as ISO 8601 without fractional seconds
        return now.ToString("yyyy-MM-ddTHH:mm:sszzz");
    }
}