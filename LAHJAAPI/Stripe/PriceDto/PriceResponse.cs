
namespace StripeGateway
{
    public class PriceResponse
    {
        public string Id { get; set; }

        public string Object { get; set; }

        public bool Active { get; set; }

        //[JsonProperty("currency")]
        public string Currency { get; set; }

        public string Interval { get; set; }

        //[JsonProperty("unit_amount_decimal")]
        public decimal UnitAmountDecimal { get; set; }

        //[JsonProperty("unit_amount")]
        public long UnitAmount { get; set; }
    }
}
