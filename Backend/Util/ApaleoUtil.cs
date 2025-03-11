using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using backend.Entities;

public class ApaleoUtil
{
    private static ApaleoUtil? _instance;
    private static readonly object Lock = new();
    private static readonly HttpClient HttpClient = new();

    private string _token = "";

    private string Token
    {
        get
        {
            if (_token == "") _token = Authenticate().Result;
            return _token;
        }
    }

    private ApaleoUtil()
    {
        HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public static ApaleoUtil GetInstance()
    {
        lock (Lock)
        {
            return _instance ??= new ApaleoUtil();
        }
    }

    private async Task<string> Authenticate()
    {
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "client_credentials")
        });

        var auth = Convert.ToBase64String("WQRH-SP-BOOKINGHERBERT:ZongcbgRr2HuYG8w2u7pn8ViuPaRdn"u8.ToArray());
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);

        var response = await HttpClient.PostAsync("https://identity.apaleo.com/connect/token", content);
        var json = await response.Content.ReadAsStringAsync();
        var token = JsonSerializer.Deserialize<ApaleoToken>(json);

        return token?.Token ?? "";
    }

    private async Task<HttpResponseMessage> SendRequest(HttpMethod method, string url, HttpContent? content = null)
    {
        using var request = new HttpRequestMessage(method, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        request.Content = content;

        var response = await HttpClient.SendAsync(request);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            _token = await Authenticate();
            return await SendRequest(method, url, content);
        }

        return response;
    }

    public async Task<Reservation?> GetReservation(string? reservationId)
    {
        if (reservationId == null) return null;

        var response = await SendRequest(HttpMethod.Get, $"https://api.apaleo.com/booking/v1/reservations/{reservationId}?expand=booker");
        var json = await response.Content.ReadAsStringAsync();

        return Reservation.FromJson(json);
    }

    public async Task<bool> CheckIn(string? reservationId)
    {
        if (await IsCheckedIn(reservationId)) return false;

        var response = await SendRequest(HttpMethod.Put, $"https://api.apaleo.com/booking/v1/reservation-actions/{reservationId}/checkin", new StringContent("", Encoding.UTF8, "application/json"));

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> CheckOut(string? reservationId)
    {
        if (!await IsCheckedIn(reservationId)) return false;

        var response = await SendRequest(HttpMethod.Put, $"https://api.apaleo.com/booking/v1/reservation-actions/{reservationId}/checkout", new StringContent("", Encoding.UTF8, "application/json"));

        return response.IsSuccessStatusCode;
    }

    private async Task<bool> IsCheckedIn(string? reservationId)
    {
        var response = await SendRequest(HttpMethod.Get, $"https://api.apaleo.com/booking/v1/reservations/{reservationId}");
        var json = await response.Content.ReadAsStringAsync();
        var reservation = Reservation.FromJson(json);

        return reservation?.Status == "InHouse";
    }

    public async Task<string?> GetReservationId(string? property, DateOnly? from, DateOnly? to, string? firstName, string? lastName)
    {
        if (property == null || from == null || to == null || firstName == null || lastName == null) return null;

        string url = $"https://api.apaleo.com/booking/v1/reservations?" +
                     $"propertyIds={property}&dateFilter=DepartureAndCheckOut&from={ConvertDate((DateOnly)from)}%3A00%3A00%2B01%3A00" +
                     $"&to={ConvertDate((DateOnly)to)}%3A00%3A00%2B01%3A00&pageNumber=1&expand=booker";

        var response = await SendRequest(HttpMethod.Get, url);
        var json = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrWhiteSpace(json))
        {
            return null;
        }

        var reservationRes = JsonSerializer.Deserialize<ReservationResponse>(json);

        return reservationRes?.Reservations.FirstOrDefault(r => r.PrimaryGuest.FirstName == firstName && r.PrimaryGuest.LastName == lastName)?.Id;
    }

    private string ConvertDate(DateOnly date)
    {
        DateTime dateTime = date.ToDateTime(new TimeOnly(15, 0)); // Set time to 15:00
        DateTimeOffset dateTimeOffset = new DateTimeOffset(dateTime, TimeZoneInfo.Local.GetUtcOffset(dateTime));

        return dateTimeOffset.ToString("yyyy-MM-ddTHH");
    }
}