using System.Text.Json;
using System.Text.Json.Serialization;

public class Reservation
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("bookingId")]
    public string BookingId { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("property")]
    public Property Property { get; set; }

    [JsonPropertyName("ratePlan")]
    public RatePlan RatePlan { get; set; }

    [JsonPropertyName("unitGroup")]
    public UnitGroup UnitGroup { get; set; }

    [JsonPropertyName("totalGrossAmount")]
    public Amount TotalGrossAmount { get; set; }

    [JsonPropertyName("arrival")]
    public DateTime Arrival { get; set; }

    [JsonPropertyName("departure")]
    public DateTime Departure { get; set; }

    [JsonPropertyName("created")]
    public DateTime Created { get; set; }

    [JsonPropertyName("modified")]
    public DateTime Modified { get; set; }

    [JsonPropertyName("adults")]
    public int Adults { get; set; }

    [JsonPropertyName("channelCode")]
    public string ChannelCode { get; set; }

    [JsonPropertyName("primaryGuest")]
    public Guest PrimaryGuest { get; set; }

    [JsonPropertyName("booker")]
    public Booker Booker { get; set; }

    [JsonPropertyName("guaranteeType")]
    public string GuaranteeType { get; set; }

    [JsonPropertyName("cancellationFee")]
    public FeeDetail CancellationFee { get; set; }

    [JsonPropertyName("noShowFee")]
    public FeeDetail NoShowFee { get; set; }

    [JsonPropertyName("balance")]
    public Amount Balance { get; set; }

    [JsonPropertyName("validationMessages")]
    public List<ValidationMessage> ValidationMessages { get; set; }

    [JsonPropertyName("allFoliosHaveInvoice")]
    public bool AllFoliosHaveInvoice { get; set; }

    [JsonPropertyName("hasCityTax")]
    public bool HasCityTax { get; set; }

    public static Reservation FromJson(string json) => 
        JsonSerializer.Deserialize<Reservation>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
}

public class Property
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}

public class RatePlan
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("isSubjectToCityTax")]
    public bool IsSubjectToCityTax { get; set; }
}

public class UnitGroup
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }
}

public class Amount
{
    [JsonIgnore]
    public decimal AmountValue { get; set; }

    [JsonPropertyName("amount")]
    public decimal AmountJson
    {
        get => AmountValue;
        set => AmountValue = value;
    }

    [JsonPropertyName("currency")]
    public string Currency { get; set; }
}

public class Guest
{
    [JsonPropertyName("firstName")]
    public string FirstName { get; set; }

    [JsonPropertyName("lastName")]
    public string LastName { get; set; }
}

public class Booker
{
    [JsonPropertyName("lastName")]
    public string LastName { get; set; }
}

public class FeeDetail
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("dueDateTime")]
    public DateTime DueDateTime { get; set; }

    [JsonPropertyName("fee")]
    public Amount Fee { get; set; }
}

public class ValidationMessage
{
    [JsonPropertyName("category")]
    public string Category { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }
}

public class ReservationResponse
{
    [JsonPropertyName("reservations")]
    public List<Reservation> Reservations { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }
}

public class ReservationDTO
{
    public string Id { get; set; }
    public string BookingId { get; set; }
    public string Status { get; set; }
    public string Property { get; set; }
    public DateTime Arrival { get; set; }
    public DateTime Departure { get; set; }
    public int Adults { get; set; }
    public string PrimaryGuest { get; set; }
    public string Booker { get; set; }

    public ReservationDTO(Reservation reservation)
    {
        Id = reservation.Id;
        BookingId = reservation.BookingId;
        Status = reservation.Status;
        Property = reservation.Property.Name;
        Arrival = reservation.Arrival;
        Departure = reservation.Departure;
        Adults = reservation.Adults;
        PrimaryGuest = $"{reservation.PrimaryGuest.FirstName} {reservation.PrimaryGuest.LastName}";
        Booker = reservation.Booker.LastName;
    }
}