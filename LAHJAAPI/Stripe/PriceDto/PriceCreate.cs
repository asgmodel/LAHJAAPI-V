using System.ComponentModel.DataAnnotations;

namespace StripeGateway
{
    public class PriceCreate
    {
        [Required]
        public string ProductId { get; set; }
        [Required]
        public string Currency { get; set; } = "usd";
        [Required]
        public long UnitAmount { get; set; }
        [Required]
        public string Interval { get; set; }
    }
}
