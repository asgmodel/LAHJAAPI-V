using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace LAHJAAPI.Dto;

public class PaymentIntentRequest
{
    [Required]
    public long Amount { get; set; }
}

public class PaymentMethodsRequest
{
    public List<string> PaymentMethodTypes { get; set; } = ["card"];
}

public class BillingInformationRequest
{

    [JsonProperty("name")]
    public required string Name { get; set; }

    [JsonProperty("email")]
    public required string Email { get; set; }

    [JsonProperty("city")]
    public required string City { get; set; }

    [JsonProperty("country")]
    public required string Country { get; set; }

    [JsonProperty("line1")]
    public string? Line1 { get; set; }

    [JsonProperty("line2")]
    public string? Line2 { get; set; }

    [JsonProperty("postal_code")]
    public string? PostalCode { get; set; }

    [JsonProperty("state")]
    public string? State { get; set; }
}
