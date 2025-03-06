using System.Text.Json;

namespace backend.Entities;

public class Reservation
{
    public string Id { get; set; }
    public string BookingId { get; set; }
    public string Status { get; set; }
    public Property Property { get; set; }
    public RatePlan RatePlan { get; set; }
    public UnitGroup UnitGroup { get; set; }
    public Money TotalGrossAmount { get; set; }
    public DateTime Arrival { get; set; }
    public DateTime Departure { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public int Adults { get; set; }
    public string ChannelCode { get; set; }
    public Guest PrimaryGuest { get; set; }
    public Guest Booker { get; set; }
    public string GuaranteeType { get; set; }
    public Fee CancellationFee { get; set; }
    public Fee NoShowFee { get; set; }
    public Money Balance { get; set; }
    public List<ValidationMessage> ValidationMessages { get; set; }
    public bool AllFoliosHaveInvoice { get; set; }
    public List<TaxDetail> TaxDetails { get; set; }
    public bool HasCityTax { get; set; }
    public PayableAmount PayableAmount { get; set; }
    
    public static Reservation? FromJson(string json)
    {
        return JsonSerializer.Deserialize<Reservation>(json);
    }
}

public class Property
{
    public string Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

public class RatePlan
{
    public string Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsSubjectToCityTax { get; set; }
}

public class UnitGroup
{
    public string Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
}

public class Money
{
    public decimal Amount { get; set; }
    public string Currency { get; set; }
}

public class Guest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

public class Fee
{
    public string Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime DueDateTime { get; set; }
    public Money FeeAmount { get; set; }
}

public class ValidationMessage
{
    public string Category { get; set; }
    public string Code { get; set; }
    public string Message { get; set; }
}

public class TaxDetail
{
    public string VatType { get; set; }
    public decimal VatPercent { get; set; }
    public Money Net { get; set; }
    public Money Tax { get; set; }
}

public class PayableAmount
{
    public Money Guest { get; set; }
}